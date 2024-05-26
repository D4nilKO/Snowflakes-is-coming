using Project.Components.Scripts.Entities.Enemies;
using Project.Components.Scripts.GameState;
using TMPro;
using UnityEngine;
using VavilichevGD.Utils.Timing;

namespace Project.Components.Scripts.Timing
{
    public class TimersView : MonoBehaviour
    {
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private GameOutcome _gameOutcome;
        
        [SerializeField] private TMP_Text _enemyTimerText;
        [SerializeField] private TMP_Text _mainTimerText;
        
        [SerializeField] private bool _isInitialized;
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void UpdateEnemyTimerDisplay(float remainingTime, TimeChangingSource changingSource)
        {
            float border = 0.2f;
            
            _enemyTimerText.text = $"{remainingTime:f2}";
            _enemyTimerText.color = remainingTime < border 
                ? Color.red : Color.white;

            if (remainingTime == 0f) 
                _enemyTimerText.text = string.Empty;
        }

        private void UpdateMainTimerText(float remainingTime, TimeChangingSource changingSource)
        {
            _mainTimerText.text = FormatTime(remainingTime);
        }

        private string FormatTime(float remainingTime)
        {
            int secondsInMinute = 60;
            
            int minutes = (int)(remainingTime / secondsInMinute);
            int seconds = (int)(remainingTime % secondsInMinute);

            return $"{minutes:00}:{seconds:00}";
        }

        private void SubscribeEvents()
        {
            if (_isInitialized)
                return;
            
            _isInitialized = true;
            
            _gameOutcome.Timer.TimerValueChanged += UpdateMainTimerText;
            _enemySpawner.Timer.TimerValueChanged += UpdateEnemyTimerDisplay;
        }

        private void UnsubscribeEvents()
        {
            if (_isInitialized == false)
                return;
            
            _gameOutcome.Timer.TimerValueChanged -= UpdateMainTimerText;
            _enemySpawner.Timer.TimerValueChanged -= UpdateEnemyTimerDisplay;
        }

        public void Init()
        {
            SubscribeEvents();
        }
    }
}