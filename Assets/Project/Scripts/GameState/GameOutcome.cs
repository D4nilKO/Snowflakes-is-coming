using System;
using Project.Services;
using Project.Timing;
using UnityEngine;
using VavilichevGD.Utils.Timing;
using VContainer;
using static Project.Data.ProgressData;
using static Project.Services.MetricaId;

namespace Project.GameState
{
    public class GameOutcome : IDisposable
    {
        private PauseHandler _pauseHandler;

        private float _timeToSurvive;

        private bool _isRevived;
        private bool _isSubscribed;

        public event Action GameIsWon;
        public event Action GameIsOver;

        [Inject]
        public void Construct(PauseHandler pauseHandler)
        {
            _pauseHandler = pauseHandler;

            SubscribeSurviveTimer();
        }

        public SyncedTimer SurviveTimer { get; private set; } = new(TimerType.OneSecTick);

        public void Dispose()
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
                Debug.LogError("player is already revived");
            }

            _isRevived = true;
            MetricaSender.SendWithId(LevelReviveId, CurrentLevelNumber.ToString());
            _pauseHandler.ForceInGamePause();
        }

        public void LostGame()
        {
            PauseHandler.Pause();
            MetricaSender.SendWithId(LevelLoseId, CurrentLevelNumber.ToString());
            GameIsOver?.Invoke();
        }

        public void WonGame()
        {
            PauseHandler.Pause();
            MetricaSender.SendWithId(LevelWonId, CurrentLevelNumber.ToString());
            IncreaseUnlockedLevelNumber();
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