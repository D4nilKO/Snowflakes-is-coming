using Project.GameState;
using UnityEngine;

namespace Project.Data
{
    public class ProgressData : MonoBehaviour
    {
        [SerializeField] private GameOutcome _gameOutcome;

        private int _maxLevelsCount;

        public int CurrentLevelNumber { get; private set; } = 1;
        public int UnlockedLevelNumber { get; private set; } = 1;

        public void IncreaseCurrentLevel()
        {
            if (CurrentLevelNumber != _maxLevelsCount && CurrentLevelNumber <= UnlockedLevelNumber)
            {
                CurrentLevelNumber++;
            }
        }

        public void SetMaxLevelsCount(int maxLevelsCount)
        {
            if (maxLevelsCount <= 0)
            {
                Debug.LogError("Максимальное количество уровней должно быть больше нуля");
                return;
            }

            _maxLevelsCount = maxLevelsCount;
        }

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
            if (_gameOutcome != null)
            {
                _gameOutcome.GameIsWon += IncreaseUnlockedLevelNumber;
            }
            else
            {
                Debug.LogError("_gameOutcome is null. Cannot subscribe to events.");
            }
        }

        private void UnsubscribeEvents()
        {
            if (_gameOutcome != null)
            {
                _gameOutcome.GameIsWon -= IncreaseUnlockedLevelNumber;
            }
        }

        private void IncreaseUnlockedLevelNumber()
        {
            if (UnlockedLevelNumber == _maxLevelsCount)
                return;

            if (UnlockedLevelNumber > CurrentLevelNumber)
                return;

            UnlockedLevelNumber = CurrentLevelNumber + 1;
        }
    }
}