using System.Collections.Generic;
using NTC.Global.Pool;
using UnityEngine;
using VavilichevGD.Utils.Timing;

namespace Project.Components.Scripts
{
    [DisallowMultipleComponent]
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyMover _enemyMover;
        [SerializeField] private Transform enemyContainer;

        [Header("Тип таймера")] [SerializeField]
        private TimerType _type;

        [Header("Время спавна врагов")] [SerializeField]
        private float timerSeconds;

        private SyncedTimer _timer;

        public List<GameObject> enemyList;
        public List<int> enemyCount;
        private List<EnemyBase> spawnedEnemyList;

        private void Awake()
        {
            _timer = new SyncedTimer(_type, timerSeconds);
            _timer.TimerFinished += OnTimerFinished;
            
            //_timer.TimerValueChanged += OnTimerValueChanged(timerSeconds);
            
            spawnedEnemyList = _enemyMover.enemies;
            
            _timer.Start();
            
            //Debug.Log(_timer.remainingSeconds);
        }

        private void OnDestroy()
        {
            _timer.TimerFinished -= OnTimerFinished;
            
            //_timer.TimerValueChanged -= OnTimerValueChanged();
        }

        private void OnTimerFinished()
        {
            SpawnEnemy();
            
            _timer.Start(timerSeconds);
            Debug.Log("Timer Finished");
        }

        private void SpawnEnemy()
        {
            var enemy = NightPool.Spawn(enemyList[0], enemyContainer);
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