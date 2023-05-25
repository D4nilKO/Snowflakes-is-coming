using System.Collections.Generic;
using NTC.Global.Pool;
using UnityEngine;
using VavilichevGD.Utils.Timing;

namespace Project.Components.Scripts
{
    [DisallowMultipleComponent]
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Тип таймера")] [SerializeField]
        private TimerType _type;

        [Header("Время спавна врагов")] [SerializeField]
        private float timerSeconds;

        private SyncedTimer _timer;

        [Space(20)] [SerializeField] private EnemyMover _enemyMover;
        [SerializeField] private Transform enemyContainer;

        [System.Serializable]
        public class EnemyTypeInfo
        {
            public GameObject enemyPrefab;
            public int maxSpawnCount;
        }

        public List<EnemyTypeInfo> enemyTypes; // Лист с информацией о врагах

        private Dictionary<GameObject, int> availableEnemyCounts; // Словарь с количеством доступных врагов каждого типа
        private int currentEnemyTypeIndex;
        
        

        private void Awake()
        {
            _timer = new SyncedTimer(_type, timerSeconds);
            _timer.TimerFinished += OnTimerFinished;

            //_timer.TimerValueChanged += OnTimerValueChanged(timerSeconds);

            _timer.Start();

            //Debug.Log(_timer.remainingSeconds);
        }

        private void Start()
        {
            InitializeAvailableEnemyCounts();
            currentEnemyTypeIndex = 0;
        }

        private void OnDestroy()
        {
            _timer.TimerFinished -= OnTimerFinished;

            //_timer.TimerValueChanged -= OnTimerValueChanged();
        }

        private void OnTimerFinished()
        {
            SpawnNextEnemy();

            _timer.Start(timerSeconds);
        }

        private void InitializeAvailableEnemyCounts()
        {
            availableEnemyCounts = new Dictionary<GameObject, int>();
            foreach (EnemyTypeInfo enemyType in enemyTypes)
            {
                availableEnemyCounts[enemyType.enemyPrefab] = enemyType.maxSpawnCount;
            }
        }

        private void SpawnNextEnemy()
        {
            if (currentEnemyTypeIndex >= enemyTypes.Count)
            {
                // Если все доступные враги закончились
                Debug.Log("Нет доступных врагов!");
                return;
            }

            var currentEnemyType = enemyTypes[currentEnemyTypeIndex];

            if (availableEnemyCounts[currentEnemyType.enemyPrefab] > 0)
            {
                SpawnEnemy(currentEnemyType.enemyPrefab);
                availableEnemyCounts[currentEnemyType.enemyPrefab]--;
            }

            if (availableEnemyCounts[currentEnemyType.enemyPrefab] == 0)
            {
                currentEnemyTypeIndex++;
            }
        }

        private void SpawnEnemy(GameObject enemyPrefab)
        {
            var enemy = NightPool.Spawn(enemyPrefab, enemyContainer);
            var enemyComponent = enemy.GetComponent<EnemyBase>();
            _enemyMover.enemies.Add(enemyComponent);
        }

        // private TimerValueChangedHandler OnTimerValueChanged(float remainingTime)
        // {
        //     // Обновить число на таймере
        //     return 
        // }
    }
}