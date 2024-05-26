using UnityEngine;

namespace Project.Components.Scripts.GameState.View
{
    public class GameOutcomeView : MonoBehaviour
    {
        [SerializeField] private GameObject _losePanel;
        [SerializeField] private GameObject _wonPanel;

        [SerializeField] private GameOutcome _gameOutcome;
        
        private void Awake()
        {
            SubscribeEvents();
        }

        private void OnDestroy()
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