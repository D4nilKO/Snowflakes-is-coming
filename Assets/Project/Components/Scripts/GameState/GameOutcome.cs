using System;
using Project.LevelSystem;
using Project.Timing;
using UnityEngine;
using VavilichevGD.Utils.Timing;

namespace Project.GameState
{
    public class GameOutcome : MonoBehaviour
    {
        private const TimerType oneSecTick = TimerType.OneSecTick;

        [SerializeField]
        private PauseHandler _pauseHandler;

        private bool _isRevived;

        private bool _isSubscribed;

        public event Action GameIsWon;
        public event Action GameIsOver;

        private float _timeToSurvive;

        public SyncedTimer SurviveTimer { get; private set; }

        private void Awake()
        {
            if (_pauseHandler == null)
            {
                Debug.LogError("_pauseHandler is null");
                return;
            }

            SurviveTimer = new SyncedTimer(oneSecTick);

            SubscribeSurviveTimer();
        }

        private void OnDestroy()
        {
            UnsubscribeSurviveTimer();
        }

        public void Initialize(float timeToSurvive)
        {
            Debug.Log("Init game outcome");

            _timeToSurvive = timeToSurvive;
            _isRevived = false;

            StartSurviveTimer();
        }

        private void StartSurviveTimer()
        {
            SurviveTimer.Start(_timeToSurvive);
        }

        public void RevivePlayer()
        {
            if (_isRevived)
            {
                Debug.LogError("player is already revived", this);
            }

            _isRevived = true;
            _pauseHandler.Resume();
        }

        public void LostGame()
        {
            _pauseHandler.Pause();

            GameIsOver?.Invoke();
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

        private void WonGame()
        {
            _pauseHandler.Pause();

            GameIsWon?.Invoke();
        }
    }
}