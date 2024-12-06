using System;
using Project.Data;
using Project.Services;
using Project.Timing;
using Sirenix.OdinInspector;
using UnityEngine;
using VavilichevGD.Utils.Timing;

namespace Project.GameState
{
    public class GameOutcome : MonoBehaviour
    {
        private const TimerType oneSecTick = TimerType.OneSecTick;

        [SerializeField]
        private PauseHandler _pauseHandler;
        
        [SerializeField]
        private ProgressData _progressData;

        private bool _isRevived;

        private bool _isSubscribed;

        public event Action GameIsWon;
        public event Action GameIsOver;

        [ShowInInspector]
        private float _timeToSurvive;

        public SyncedTimer SurviveTimer { get; private set; } = new SyncedTimer(oneSecTick);

        private void Awake()
        {
            this.ValidateSerializedFields();
            
            SubscribeSurviveTimer();
        }

        private void OnDestroy()
        {
            UnsubscribeSurviveTimer();
        }

        public void Initialize(float timeToSurvive)
        {
            _timeToSurvive = timeToSurvive;
            _isRevived = false;

            StartSurviveTimer();
        }

        public void RevivePlayer()
        {
            if (_isRevived)
            {
                Debug.LogError("player is already revived", this);
            }

            _isRevived = true;
            MetricaSender.SendWithId( MetricaId.LevelReviveId, _progressData.CurrentLevelNumber.ToString());
            _pauseHandler.ForceInGamePause();
        }

        public void LostGame()
        {
            PauseHandler.Pause();
            MetricaSender.SendWithId(MetricaId.LevelLoseId, _progressData.CurrentLevelNumber.ToString());
            GameIsOver?.Invoke();
        }

        private void WonGame()
        {
            PauseHandler.Pause();
            MetricaSender.SendWithId(MetricaId.LevelWonId, _progressData.CurrentLevelNumber.ToString());
            GameIsWon?.Invoke();
        }

        private void StartSurviveTimer()
        {
            SurviveTimer.Restart(_timeToSurvive);
        }

        private void SubscribeSurviveTimer()
        {
            if (_isSubscribed)
                return;

            _isSubscribed = true;
            SurviveTimer.TimerFinished += WonGame;
        }

        private void UnsubscribeSurviveTimer()
        {
            if (_isSubscribed == false)
                return;

            _isSubscribed = false;
            SurviveTimer.TimerFinished -= WonGame;
        }
    }
}