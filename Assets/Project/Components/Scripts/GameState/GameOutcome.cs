using System;
using Project.LevelSystem;
using Project.Timing;
using UnityEngine;
using VavilichevGD.Utils.Timing;

namespace Project.GameState
{
    public class GameOutcome : MonoBehaviour
    {
        [SerializeField]
        private PauseHandler _pauseHandler;

        [SerializeField]
        private Game _game;

        [SerializeField]
        private bool _isWonGame;

        private bool _isSubscribed;

        public event Action GameIsWon;
        public event Action GameIsOver;

        private float _timeToSurvive;

        public SyncedTimer SurviveTimer { get; private set; }

        private void Awake()
        {
            if (_game == null || _pauseHandler == null)
            {
                Debug.LogError("GameOutcome: _game or _pauseHandler is null");
                return;
            }

            SurviveTimer = new SyncedTimer(TimerType.OneSecTick);

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

            StartSurviveTimer();
        }

        private void StartSurviveTimer()
        {
            SurviveTimer.SetTime(_timeToSurvive);
            SurviveTimer.Start();
        }

        public void LostGame()
        {
            _isWonGame = false;
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
            _isWonGame = true;
            _pauseHandler.Pause();

            GameIsWon?.Invoke();
        }
    }
}