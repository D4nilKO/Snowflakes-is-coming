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

        [SerializeField] private bool _isInitialized;

        public SyncedTimer Timer { get; private set; }

        public event Action GameIsWon;
        public event Action GameIsOver;

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            if (_isInitialized)
                return;

            _isInitialized = true;

            Timer.TimerFinished += WonGame;
        }

        private void UnsubscribeEvents()
        {
            if (_isInitialized == false)
                return;

            Timer.TimerFinished -= WonGame;
        }

        private void WonGame()
        {
            _isWonGame = true;
            _pauseHandler.Pause();
            GameIsWon?.Invoke();
        }

        public void LostGame()
        {
            _isWonGame = false;
            _pauseHandler.Pause();
            GameIsOver?.Invoke();
        }

        public void Init(float timeToSurvive)
        {
            Debug.Log("init game outcome");
            
            Timer = new SyncedTimer(TimerType.OneSecTick, timeToSurvive);
            Timer.Start();
            
            SubscribeEvents();
        }
    }
}