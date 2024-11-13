using System;
using System.Collections.Generic;
using System.IO;
using Project.LevelSystem.LevelStructure;
using UnityEngine;
using VavilichevGD.Utils.Timing;
using static NTC.Global.Pool.NightPool;

namespace Project.Entities.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private string _enemyPrefabFolder;

        [SerializeField]
        private EntityMover _entityMover;

        [SerializeField]
        private Transform _enemyContainer;

        [SerializeField]
        private float _initialSpawnDelay;

        private IReadOnlyList<EnemyTypeInfo> _enemyTypes;
        private Dictionary<string, int> _availableEnemyCounts;

        private TimerType _timerType = TimerType.UpdateTick;
        private int _spawnDelaySeconds;

        private int _currentEnemyTypeIndex;

        public SyncedTimer Timer { get; private set; }

        private void Awake()
        {
            SubscribeToTimer();
        }

        private void OnDestroy()
        {
            UnsubscribeFromTimer();
        }

        public void Initialize(IReadOnlyList<EnemyTypeInfo> enemyTypeInfos, int timeToSpawn)
        {
            Debug.Log("init enemy spawner");

            gameObject.SetActive(true);

            SetStartedParameters(enemyTypeInfos, timeToSpawn);
            _currentEnemyTypeIndex = 0;

            InitializeAvailableEnemyCounts();

            Timer = new SyncedTimer(_timerType, _spawnDelaySeconds);
            Timer.Start(_initialSpawnDelay);
        }

        private void OnTimerFinished()
        {
            SpawnNextEnemy();

            if (_currentEnemyTypeIndex >= _enemyTypes.Count)
            {
                Debug.Log("All enemies spawned");
                gameObject.SetActive(false);
                return;
            }

            Timer.Start(_spawnDelaySeconds);
        }

        private void InitializeAvailableEnemyCounts()
        {
            _availableEnemyCounts = new Dictionary<string, int>();

            foreach (EnemyTypeInfo enemyType in _enemyTypes)
            {
                _availableEnemyCounts[enemyType.EnemyPrefabName] = enemyType.MaxSpawnCount;
            }
        }

        private void SpawnNextEnemy()
        {
            EnemyTypeInfo currentEnemyType = _enemyTypes[_currentEnemyTypeIndex];
            string enemyPrefabName = currentEnemyType.EnemyPrefabName;

            if (_availableEnemyCounts.TryGetValue(enemyPrefabName, out int availableCount) && availableCount > 0)
            {
                string path = Path.Combine(_enemyPrefabFolder, enemyPrefabName);
                GameObject enemyPrefab = Resources.Load<GameObject>(path);

                if (enemyPrefab != null)
                {
                    SpawnEnemy(enemyPrefab);
                    _availableEnemyCounts[enemyPrefabName]--;
                }
                else
                {
                    Debug.LogError($"Не удалось загрузить префаб врага: {path}");
                    return;
                }
            }

            if (_availableEnemyCounts.TryGetValue(enemyPrefabName, out int remainingCount) && remainingCount == 0)
                _currentEnemyTypeIndex++;
        }

        private void SubscribeToTimer()
        {
            Timer.TimerFinished += OnTimerFinished;
        }

        private void UnsubscribeFromTimer()
        {
            Timer.TimerFinished -= OnTimerFinished;
        }

        private void SpawnEnemy(GameObject enemyPrefab)
        {
            GameObject enemy = Spawn(enemyPrefab, _enemyContainer);
            EnemyBase enemyComponent = enemy.GetComponent<EnemyBase>();
            _entityMover.AddMovableEntity(enemyComponent);
        }

        private void SetStartedParameters(IReadOnlyList<EnemyTypeInfo> enemyTypeInfos, int timeToSpawn)
        {
            _enemyTypes = enemyTypeInfos;
            _spawnDelaySeconds = timeToSpawn;
        }
    }
}