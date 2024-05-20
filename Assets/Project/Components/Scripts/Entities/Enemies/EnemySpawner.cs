using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using VavilichevGD.Utils.Timing;
using static NTC.Global.Pool.NightPool;

namespace Project.Components.Scripts.Entities.Enemies
{
    [DisallowMultipleComponent]
    public class EnemySpawner : MonoBehaviour
    {
        [FormerlySerializedAs("timerType")] [Header("Тип таймера")] [SerializeField] private TimerType _timerType;
        [FormerlySerializedAs("enemyContainer")] [SerializeField] private Transform _enemyContainer;
        [FormerlySerializedAs("folder")] [SerializeField] private string _folder;

        [SerializeField] private float _timeToSpawnFirstEnemy = 0.5f;

        [SerializeField] private TimerViewer timerViewer;

        [SerializeField] private EntityMover entityMover;
        
        private SyncedTimer enemyTimer;
        public float timerSeconds; // поменять на private
        
        [HideInInspector] public List<EnemyTypeInfo> enemyTypes; // тоже на private
        private Dictionary<string, int> _availableEnemyCounts;
        
        private int _currentEnemyTypeIndex;

        private void Awake()
        {
            entityMover = FindObjectOfType<EntityMover>();// эти две строчки тоже убрать по возможности
            timerViewer = FindObjectOfType<TimerViewer>();

            enemyTimer = new SyncedTimer(_timerType, timerSeconds);

            SubscribeEvents();
        }

        private void Start()
        {
            InitializeAvailableEnemyCounts();
            enemyTimer.Start(_timeToSpawnFirstEnemy);
        }

        private void SubscribeEvents()
        {
            enemyTimer.TimerFinished += OnTimerFinished;
            enemyTimer.TimerValueChanged += TimerValueChanged;
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            enemyTimer.TimerFinished -= OnTimerFinished;
            enemyTimer.TimerValueChanged -= TimerValueChanged;
        }

        private void OnTimerFinished()
        {
            SpawnNextEnemy();

            if (_currentEnemyTypeIndex >= enemyTypes.Count)
            {
                Debug.Log("Нет доступных врагов!");
                gameObject.SetActive(false);
                return;
            }

            enemyTimer.Start(timerSeconds);
        }

        private void InitializeAvailableEnemyCounts()
        {
            _availableEnemyCounts = new Dictionary<string, int>();

            foreach (EnemyTypeInfo enemyType in enemyTypes)
            {
                _availableEnemyCounts[enemyType.EnemyPrefabName] = enemyType.MaxSpawnCount;
            }
        }

        private void SpawnNextEnemy()
        {
            EnemyTypeInfo currentEnemyType = enemyTypes[_currentEnemyTypeIndex];
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
                    Debug.LogError("Не удалось загрузить префаб врага: " + path);
                }
            }

            if (_availableEnemyCounts.TryGetValue(enemyPrefabName, out int remainingCount) && remainingCount == 0)
                _currentEnemyTypeIndex++;
        }

        private void SpawnEnemy(GameObject enemyPrefab)
        {
            GameObject enemy = Spawn(enemyPrefab, _enemyContainer);
            EnemyBase enemyComponent = enemy.GetComponent<EnemyBase>();
            entityMover.AddEnemy(enemyComponent);
        }

        private void TimerValueChanged(float remainingSeconds, TimeChangingSource timeChangingSource)
        {
            timerViewer.UpdateEnemyTimerText(remainingSeconds);
        }
    }
}