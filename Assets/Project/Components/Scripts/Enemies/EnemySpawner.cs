using System;
using System.Collections.Generic;
using System.IO;
using NTC.Global.Pool;
using UnityEngine;
using UnityEngine.Serialization;
using VavilichevGD.Utils.Timing;

namespace Project.Components.Scripts.Enemies
{
    [DisallowMultipleComponent]
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Тип таймера")] [SerializeField]
        private TimerType timerType;

        [HideInInspector] public float timerSeconds;

        private SyncedTimer enemyTimer;
        private TimerViewer timerViewer;

        private EntityMover entityMover;

        [SerializeField] private Transform enemyContainer;

        [HideInInspector] public List<EnemyTypeInfo> enemyTypes;

        private Dictionary<string, int> availableEnemyCounts;
        private int currentEnemyTypeIndex;

        private void Awake()
        {
            entityMover = FindObjectOfType<EntityMover>();
            timerViewer = FindObjectOfType<TimerViewer>();
            enemyTimer = new SyncedTimer(timerType, timerSeconds);
            enemyTimer.TimerFinished += OnTimerFinished;
            enemyTimer.TimerValueChanged += TimerValueChanged;
        }

        private void Start()
        {
            InitializeAvailableEnemyCounts();
            enemyTimer.Start();
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
            availableEnemyCounts = new Dictionary<string, int>();

            foreach (EnemyTypeInfo enemyType in enemyTypes)
            {
                availableEnemyCounts[enemyType.enemyPrefabName] = enemyType.maxSpawnCount;
            }
        }

        private void SpawnNextEnemy()
        {
            var currentEnemyType = enemyTypes[currentEnemyTypeIndex];
            var enemyPrefabName = currentEnemyType.enemyPrefabName;

            if (availableEnemyCounts.TryGetValue(enemyPrefabName, out var availableCount) && availableCount > 0)
            {
                var path = Path.Combine("Enemies", enemyPrefabName);
                var enemyPrefab = Resources.Load<GameObject>(path);

                if (enemyPrefab != null)
                {
                    SpawnEnemy(enemyPrefab);
                    availableEnemyCounts[enemyPrefabName]--;
                }
                else
                {
                    Debug.LogError("Не удалось загрузить префаб врага: " + path);
                }
            }

            if (availableEnemyCounts.TryGetValue(enemyPrefabName, out var remainingCount) && remainingCount == 0)
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