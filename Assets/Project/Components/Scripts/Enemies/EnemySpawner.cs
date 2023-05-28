using System.Collections.Generic;
using NTC.Global.Pool;
using UnityEngine;
using VavilichevGD.Utils.Timing;

namespace Project.Components.Scripts.Enemies
{
    [DisallowMultipleComponent]
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Тип таймера")] [SerializeField]
        private TimerType _type;

        [Header("Время спавна врагов")] [SerializeField]
        public float timerSeconds;

        private SyncedTimer enemyTimer;
        private TimerViewer timerViewer;

        private EntityMover entityMover;
        [SerializeField] private Transform enemyContainer;

        public List<EnemyTypeInfo> enemyTypes; // Лист с информацией о врагах

        private Dictionary<GameObject, int> availableEnemyCounts; // Словарь с количеством доступных врагов каждого типа
        private int currentEnemyTypeIndex;

        private void Awake()
        {
            entityMover = FindObjectOfType<EntityMover>();
            timerViewer = FindObjectOfType<TimerViewer>();
            enemyTimer = new SyncedTimer(_type, timerSeconds);
            enemyTimer.TimerFinished += OnTimerFinished;

            enemyTimer.TimerValueChanged += TimerValueChanged;

            enemyTimer.Start();
        }

        private void Start()
        {
            InitializeAvailableEnemyCounts();
            currentEnemyTypeIndex = 0;
        }

        private void OnDestroy()
        {
            enemyTimer.TimerFinished -= OnTimerFinished;

            enemyTimer.TimerValueChanged -= TimerValueChanged;
        }

        private void OnTimerFinished()
        {
            SpawnNextEnemy();
            
            if (currentEnemyTypeIndex >= enemyTypes.Count)
            {
                Debug.Log("Нет доступных врагов!");
                gameObject.SetActive(false);
                return;
            }
            
            enemyTimer.Start(timerSeconds);
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
            entityMover.enemies.Add(enemyComponent);
        }


        private void TimerValueChanged(float remainingSeconds, TimeChangingSource timeChangingSource)
        {
            timerViewer.UpdateEnemyTimerText(remainingSeconds);
        }
    }
}