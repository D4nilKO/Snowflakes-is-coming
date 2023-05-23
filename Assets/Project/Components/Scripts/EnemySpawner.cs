using System;
using NTC.Global.Pool;
using UnityEngine;
using VavilichevGD.Utils.Timing;

namespace Project.Components.Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private TimerType _type;
        [SerializeField] private float timerSeconds;

        private SyncedTimer _timer;

        private void Awake()
        {
            _timer = new SyncedTimer(_type, timerSeconds);
            _timer.TimerFinished += OnTimerFinished;
            //_timer.TimerValueChanged += OnTimerValueChanged();

        }

        private void OnTimerFinished()
        {
            //NightPool.Spawn();
        }

        private void OnTimerValueChanged()
        {
            // Обновить число на таймере
        }
        
        
    }
}