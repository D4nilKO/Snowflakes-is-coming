using System;
using System.Collections.Generic;
using System.Linq;
using Project.LevelSystem.LevelStructure;
using UnityEngine;
using VavilichevGD.Utils.Timing;
using VContainer;
using static NTC.Global.Pool.NightPool;

namespace Project.Entities.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        private const TimerType updateTick = TimerType.UpdateTick;

        [SerializeField, Min(0.5f)]
        private float _initialSpawnDelay;

        private EntityMover _entityMover;
        private EnemyContainer _enemyContainer;

        private List<EnemyTypeInfo> _enemyTypes;
        private Dictionary<string, int> _availableEnemyCounts;

        private int _spawnDelaySeconds;

        private int _currentEnemyTypeIndex;

        public event Action<float> Initialized;

        public SyncedTimer Timer { get; private set; }

        [Inject]
        public void Construct(EntityMover mover, EnemyContainer container)
        {
            _entityMover = mover;
            _enemyContainer = container;

            SetStartTransform();
        }

        public void Initialize(List<EnemyTypeInfo> enemyTypeInfos, int timeToSpawn)
        {
            gameObject.SetActive(true);

            SetStartedParameters(enemyTypeInfos, timeToSpawn);
            _currentEnemyTypeIndex = 0;

            InitializeAvailableEnemyCounts();

            Timer.Restart(_initialSpawnDelay);

            Initialized?.Invoke(_initialSpawnDelay);
        }

        private void Awake()
        {
            Timer = new SyncedTimer(updateTick);
            SubscribeToTimer();
        }

        private void OnDestroy()
        {
            UnsubscribeFromTimer();
        }

        private void SetStartTransform()
        {
            transform.SetParent(_enemyContainer.transform);
            transform.localPosition = Vector3.zero;
        }

        private void OnTimerFinished()
        {
            SpawnNextEnemy();

            if (_currentEnemyTypeIndex >= _enemyTypes.Count)
            {
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
                string path = enemyPrefabName;
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
            GameObject enemy = Spawn(enemyPrefab, _enemyContainer.transform);
            EnemyBase enemyComponent = enemy.GetComponent<EnemyBase>();
            _entityMover.AddMovableEntity(enemyComponent);
        }

        private void SetStartedParameters(IEnumerable<EnemyTypeInfo> enemyTypeInfos, int timeToSpawn)
        {
            _enemyTypes = enemyTypeInfos.ToList();
            _spawnDelaySeconds = timeToSpawn;
        }
    }
}