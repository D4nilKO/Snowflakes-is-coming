using System;
using Project.Data;
using Project.Entities.Character;
using Project.Services;
using Project.Timing;
using UnityEngine;
using VavilichevGD.Utils.Timing;
using VContainer;
using static Project.Services.MetricaId;

namespace Project.GameState
{
    public class GameOutcome : IDisposable
    {
        private PauseHandler _pauseHandler;
        private ProgressData _progressData;
        private CharacterCollisionHandler _collisionHandler;

        private float _timeToSurvive;

        private bool _isRevived;
        private bool _isSubscribed;

        public event Action GameIsWon;
        public event Action GameIsOver;

        public SyncedTimer SurviveTimer { get; private set; } = new(TimerType.OneSecTick);

        [Inject]
        public void Construct(PauseHandler pauseHandler, ProgressData progressData,
            CharacterCollisionHandler collisionHandler)
        {
            _pauseHandler = pauseHandler;
            _progressData = progressData;
            _collisionHandler = collisionHandler;

            SubscribeSurviveTimer();
            SubscribeCollisionHandler();
        }

        public void Dispose()
        {
            UnsubscribeSurviveTimer();
            UnsubscribeCollisionHandler();
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
            _collisionHandler.ActivateInvincibility(1f);
            _pauseHandler.InGamePause();

            MetricaSender.SendWithId(LevelReviveId, _progressData.CurrentLevelNumber.ToString());
        }

        public void LostGame()
        {
            _pauseHandler.Pause();
            MetricaSender.SendWithId(LevelLoseId, _progressData.CurrentLevelNumber.ToString());
            GameIsOver?.Invoke();
        }

        public void WonGame()
        {
            _pauseHandler.Pause();
            MetricaSender.SendWithId(LevelWonId, _progressData.CurrentLevelNumber.ToString());
            _progressData.IncreaseUnlockedLevelNumber();
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

        private void SubscribeCollisionHandler()
        {
            _collisionHandler.DamageTaken += LostGame;
        }

        private void UnsubscribeCollisionHandler()
        {
            _collisionHandler.DamageTaken -= LostGame;
        }
    }
}