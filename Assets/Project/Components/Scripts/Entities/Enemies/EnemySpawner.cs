using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VavilichevGD.Utils.Timing;
using static NTC.Global.Pool.NightPool;

namespace Project.Components.Scripts.Entities.Enemies
{
    [DisallowMultipleComponent]
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Тип таймера")] [SerializeField]
        private TimerType _timerType;

        [SerializeField] private Transform _enemyContainer;
        [SerializeField] private string _folder;

        [SerializeField] private float _timeToSpawnFirstEnemy = 0.5f;
        
        [SerializeField] private EntityMover _entityMover;

        [SerializeField] private List<EnemyTypeInfo> _enemyTypes;
        
        private Dictionary<string, int> _availableEnemyCounts;
        public SyncedTimer Timer { get; private set; }
        private int _timerSeconds;

        private int _currentEnemyTypeIndex;
        
        [SerializeField] private bool _isInitialized;

        public void Init(List<EnemyTypeInfo> enemyTypeInfos, int timeToSpawn)
        {
            Debug.Log("init enemy spawner");
            
            SetStartedParameters(enemyTypeInfos, timeToSpawn);
            _currentEnemyTypeIndex = 0;
            
            InitializeAvailableEnemyCounts();
            
            Timer = new SyncedTimer(_timerType, _timerSeconds);
            Timer.Start(_timeToSpawnFirstEnemy);
            
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void OnTimerFinished()
        {
            SpawnNextEnemy();

            if (_currentEnemyTypeIndex >= _enemyTypes.Count)
            {
                Debug.Log("Нет доступных врагов");
                gameObject.SetActive(false);
                return;
            }

            Timer.Start(_timerSeconds);
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
                string path = Path.Combine(_folder, enemyPrefabName);
                GameObject enemyPrefab = Resources.Load<GameObject>(path);

                if (enemyPrefab != null)
                {
                    SpawnEnemy(enemyPrefab);
                    _availableEnemyCounts[enemyPrefabName]--;
                }
                else
                {
                    Debug.LogError($"Не удалось загрузить префаб врага: {path}");
                }
            }

            if (_availableEnemyCounts.TryGetValue(enemyPrefabName, out int remainingCount) && remainingCount == 0)
                _currentEnemyTypeIndex++;
        }

        private void SubscribeEvents()
        {
            Timer.TimerFinished += OnTimerFinished;
        }

        private void UnsubscribeEvents()
        {
            if (_isInitialized) 
                Timer.TimerFinished -= OnTimerFinished;
        }

        private void SpawnEnemy(GameObject enemyPrefab)
        {
            GameObject enemy = Spawn(enemyPrefab, _enemyContainer);
            EnemyBase enemyComponent = enemy.GetComponent<EnemyBase>();
            _entityMover.AddEnemy(enemyComponent);
        }

        private void SetStartedParameters(List<EnemyTypeInfo> enemyTypeInfos, int timeToSpawn)
        {
            _enemyTypes = enemyTypeInfos;
            _timerSeconds = timeToSpawn;
        }
    }
}