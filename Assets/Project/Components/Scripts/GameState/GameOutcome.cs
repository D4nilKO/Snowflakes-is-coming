using System;
using Project.Components.Scripts.Timing;
using UnityEngine;
using VavilichevGD.Utils.Timing;

namespace Project.Components.Scripts.GameState
{
    public class GameOutcome : MonoBehaviour
    {
        [SerializeField] private PauseHandler _pauseHandler;

        [SerializeField] private bool _isWonGame;

        private bool _isSubscribed;

        public event Action GameIsWon;
        public event Action GameIsOver;

        public SyncedTimer Timer { get; private set; }

        public void Initialize(float timeToSurvive)
        {
            Debug.Log("init game outcome");

            UnsubscribeEvents();

            Timer = new SyncedTimer(TimerType.OneSecTick, timeToSurvive);
            Timer.Start();

            SubscribeEvents();
        }

        public void LostGame()
        {
            _isWonGame = false;
            _pauseHandler.Pause();

            GameIsOver?.Invoke();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            if (_isSubscribed)
                return;

            _isSubscribed = true;

            Timer.TimerFinished += WonGame;

            Debug.Log("subscribe events");
        }

        private void UnsubscribeEvents()
        {
            if (_isSubscribed == false)
                return;

            _isSubscribed = false;

            Timer.TimerFinished -= WonGame;

            Debug.Log("unsubscribe events");
        }

        private void WonGame()
        {
            _isWonGame = true;
            _pauseHandler.Pause();

            GameIsWon?.Invoke();
        }
    }
}