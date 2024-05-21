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
        [Header("Тип таймера")] [SerializeField] private TimerType _timerType;
        [SerializeField] private Transform _enemyContainer;
        [SerializeField] private string _folder;

        [SerializeField] private float _timeToSpawnFirstEnemy = 0.5f;

        [SerializeField] private TimerViewer _timerViewer;
        [SerializeField] private EntityMover _entityMover;
        
        [SerializeField] private List<EnemyTypeInfo> _enemyTypes;

        private SyncedTimer _enemyTimer;
        private int _timerSeconds;

        private Dictionary<string, int> _availableEnemyCounts;
        
        private int _currentEnemyTypeIndex;

        private void Awake()
        {
            _entityMover = FindObjectOfType<EntityMover>();// эти две строчки тоже убрать
            _timerViewer = FindObjectOfType<TimerViewer>();

            _enemyTimer = new SyncedTimer(_timerType, _timerSeconds);

            SubscribeEvents();
        }

        private void Start()
        {
            InitializeAvailableEnemyCounts();
            _enemyTimer.Start(_timeToSpawnFirstEnemy);
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _enemyTimer.TimerFinished += OnTimerFinished;
            _enemyTimer.TimerValueChanged += TimerValueChanged;
        }

        private void UnsubscribeEvents()
        {
            _enemyTimer.TimerFinished -= OnTimerFinished;
            _enemyTimer.TimerValueChanged -= TimerValueChanged;
        }

        private void OnTimerFinished()
        {
            SpawnNextEnemy();

            if (_currentEnemyTypeIndex >= _enemyTypes.Count)
            {
                Debug.Log("Нет доступных врагов!");
                gameObject.SetActive(false);
                return;
            }

            _enemyTimer.Start(_timerSeconds);
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

        private void SpawnEnemy(GameObject enemyPrefab)
        {
            GameObject enemy = Spawn(enemyPrefab, _enemyContainer);
            EnemyBase enemyComponent = enemy.GetComponent<EnemyBase>();
            _entityMover.AddEnemy(enemyComponent);
        }

        private void TimerValueChanged(float remainingSeconds, TimeChangingSource timeChangingSource)
        {
            _timerViewer.UpdateEnemyTimerText(remainingSeconds);
        }

        public void SetStartedParameters(List<EnemyTypeInfo> enemyTypeInfos, int timeToSpawn)
        {
            _enemyTypes = enemyTypeInfos;
            _timerSeconds = timeToSpawn;
        }
    }
}