using System;
using Project.Architecture.UI;
using UnityEngine;
using VContainer;

namespace Project.GameState.View
{
    public class GameOutcomeView : IDisposable
    {
        private GameObject _losePanel;
        private GameObject _wonPanel;

        private GameOutcome _gameOutcome;

        [Inject]
        public void Construct(GameOutcome gameOutcome, WonPanel wonPanel, LosePanel losePanel)
        {
            _gameOutcome = gameOutcome;
            _wonPanel = wonPanel.gameObject;
            _losePanel = losePanel.gameObject;

            SubscribeEvents();
        }

        public void Dispose()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _gameOutcome.GameIsWon += ShowWon;
            _gameOutcome.GameIsOver += ShowLose;
        }

        private void UnsubscribeEvents()
        {
            _gameOutcome.GameIsWon -= ShowWon;
            _gameOutcome.GameIsOver -= ShowLose;
        }

        private void ShowWon()
        {
            _wonPanel.SetActive(true);
        }

        private void ShowLose()
        {
            _losePanel.SetActive(true);
        }
    }
}