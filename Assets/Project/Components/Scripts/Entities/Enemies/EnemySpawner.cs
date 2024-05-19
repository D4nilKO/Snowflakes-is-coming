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
        [FormerlySerializedAs("timerType")] [Header("Тип таймера")] [SerializeField]
        private TimerType _timerType;

        [FormerlySerializedAs("enemyContainer")] [SerializeField]
        private Transform _enemyContainer;

        [FormerlySerializedAs("folder")] [SerializeField]
        private string _folder;

        public float timerSeconds; // поменять на private
        [SerializeField] private float _timeToSpawnFirstEnemy = 0.5f; 

        private SyncedTimer enemyTimer;
        [SerializeField] private TimerViewer timerViewer;

        [SerializeField] private EntityMover entityMover;

        [HideInInspector] public List<EnemyTypeInfo> enemyTypes; // тоже на private

        private Dictionary<string, int> availableEnemyCounts;
        private int currentEnemyTypeIndex;

        private void Awake()
        {
            entityMover = FindObjectOfType<EntityMover>();// эти две строчки тоже убрать по возможности
            timerViewer = FindObjectOfType<TimerViewer>();

            enemyTimer = new SyncedTimer(_timerType, timerSeconds);

            enemyTimer.TimerFinished += OnTimerFinished;
            enemyTimer.TimerValueChanged += TimerValueChanged;
        }

        private void Start()
        {
            InitializeAvailableEnemyCounts();
            enemyTimer.Start(0.5f);
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
            EnemyTypeInfo currentEnemyType = enemyTypes[currentEnemyTypeIndex];
            string enemyPrefabName = currentEnemyType.enemyPrefabName;

            if (availableEnemyCounts.TryGetValue(enemyPrefabName, out var availableCount) && availableCount > 0)
            {
                string path = Path.Combine(_folder, enemyPrefabName);
                GameObject enemyPrefab = Resources.Load<GameObject>(path);

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

        private void SpawnEnemy(GameObject enemyPrefab) // тут еще попробовать строчку убрать
        {
            GameObject enemy = NightPool.Spawn(enemyPrefab, _enemyContainer);
            EnemyBase enemyComponent = enemy.GetComponent<EnemyBase>();
            entityMover.enemies.Add(enemyComponent);
        }

        private void TimerValueChanged(float remainingSeconds, TimeChangingSource timeChangingSource)
        {
            timerViewer.UpdateEnemyTimerText(remainingSeconds);
        }
    }
}