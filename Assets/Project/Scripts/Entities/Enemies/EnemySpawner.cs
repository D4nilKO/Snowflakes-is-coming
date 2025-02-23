using System;
using System.Collections.Generic;
using System.Linq;
using NTC.Global.Pool;
using Project.LevelSystem.LevelStructure;
using UnityEngine;
using VavilichevGD.Utils.Timing;
using VContainer;

namespace Project.Entities.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        private const TimerType updateTick = TimerType.UpdateTick;

        [SerializeField, Min(0.5f)]
        private float _initialSpawnDelay = 1f;

        private EntityRotator _entityRotator;
        private EnemyContainer _enemyContainer;

        private List<EnemyTypeInfo> _enemyTypes;
        private Dictionary<string, int> _availableEnemyCounts;

        private int _spawnDelaySeconds;

        private int _currentEnemyTypeIndex;

        public event Action<float> Initialized;

        public SyncedTimer Timer { get; private set; }

        [Inject]
        public void Construct(EntityRotator rotator, EnemyContainer container)
        {
            _entityRotator = rotator;
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
            EnemyTypeInfo currentType = _enemyTypes[_currentEnemyTypeIndex];
            string prefabName = currentType.EnemyPrefabName;

            bool hasEnemy = _availableEnemyCounts.TryGetValue(prefabName, out int availableCount);
            bool isAvailable = availableCount > 0;

            if (hasEnemy && isAvailable)
            {
                var enemyPrefab = Resources.Load<EnemyBase>(prefabName);

                if (enemyPrefab == null)
                {
                    Debug.LogError($"Не удалось загрузить префаб врага: {prefabName}");
                    return;
                }

                SpawnEnemy(enemyPrefab);
                _availableEnemyCounts[prefabName]--;
            }

            hasEnemy = _availableEnemyCounts.TryGetValue(prefabName, out int remainingCount);
            isAvailable = remainingCount > 0;

            if (hasEnemy && isAvailable == false)
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

        private void SpawnEnemy(EnemyBase enemyPrefab)
        {
            EnemyBase enemy = NightPool.Spawn(enemyPrefab, _enemyContainer.transform);
            _entityRotator.AddEntity(enemy);
            _enemyContainer.AddEnemy(enemy);
        }

        private void SetStartedParameters(IEnumerable<EnemyTypeInfo> enemyTypeInfos, int timeToSpawn)
        {
            _enemyTypes = enemyTypeInfos.ToList();
            _spawnDelaySeconds = timeToSpawn;
        }
    }
}