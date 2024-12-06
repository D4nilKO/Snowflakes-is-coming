using Project.Entities.Enemies;
using Project.GameState;
using TMPro;
using UnityEngine;
using VavilichevGD.Utils.Timing;

namespace Project.Timing
{
    public class TimersView : MonoBehaviour
    {
        // Подправить
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
            
            _gameOutcome.SurviveTimer.TimerValueChanged += UpdateMainTimerText;
            _enemySpawner.Timer.TimerValueChanged += UpdateEnemyTimerDisplay;
        }

        private void UnsubscribeEvents()
        {
            if (_isInitialized == false)
                return;
            
            _isInitialized = false;
            
            _gameOutcome.SurviveTimer.TimerValueChanged -= UpdateMainTimerText;
            _enemySpawner.Timer.TimerValueChanged -= UpdateEnemyTimerDisplay;
        }

        public void Initialize(float timeToSurvive)
        {
            UnsubscribeEvents();
            SubscribeEvents();
            
            UpdateMainTimerText(timeToSurvive, default);
        }
    }
}