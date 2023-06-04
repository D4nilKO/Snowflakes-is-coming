using System;
using System.Collections.Generic;
using NTC.Global.Pool;
using UnityEngine;
using UnityEngine.Serialization;
using VavilichevGD.Utils.Timing;

namespace Project.Components.Scripts.Enemies
{
    
    [DisallowMultipleComponent]
    public class EnemySpawner : MonoBehaviour
    {
        [FormerlySerializedAs("_type")] [Header("Тип таймера")] [SerializeField]
        private TimerType timerType;

        [NonSerialized] public float timerSeconds;

        private SyncedTimer enemyTimer;
        private TimerViewer timerViewer;

        private EntityMover entityMover;
        [SerializeField] private Transform enemyContainer;

        [NonSerialized] public List<EnemyTypeInfo> enemyTypes;

        private Dictionary<GameObject, int> availableEnemyCounts;
        private int currentEnemyTypeIndex;

        private void Awake()
        {
            entityMover = FindObjectOfType<EntityMover>();
            timerViewer = FindObjectOfType<TimerViewer>();
            enemyTimer = new SyncedTimer(timerType, timerSeconds);
            enemyTimer.TimerFinished += OnTimerFinished;

            enemyTimer.TimerValueChanged += TimerValueChanged;

            enemyTimer.Start();
        }

        private void Start()
        {
            InitializeAvailableEnemyCounts();
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