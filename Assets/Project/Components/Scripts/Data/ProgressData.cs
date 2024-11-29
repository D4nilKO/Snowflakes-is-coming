using Project.GameState;
using UnityEngine;
using YG;

namespace Project.Data
{
    public class ProgressData : MonoBehaviour
    {
        [SerializeField]
        private GameOutcome _gameOutcome;

        private int _maxLevelsCount;

        public int CurrentLevelNumber { get; private set; } = 1;
        public int UnlockedLevelNumber { get; private set; } = 1;

        #region Public methods

        public void IncreaseCurrentLevel()
        {
            if (CurrentLevelNumber != _maxLevelsCount && CurrentLevelNumber <= UnlockedLevelNumber)
                CurrentLevelNumber++;
        }

        public void SetMaxLevelsCount(int maxLevelsCount)
        {
            if (maxLevelsCount <= 0)
            {
                Debug.LogError("Максимальное количество уровней должно быть больше нуля");
                return;
            }

            _maxLevelsCount = maxLevelsCount;
        }

        #endregion

        private void Start()
        {
            if (YandexGame.SDKEnabled)
                GetLoad();
        }

        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnsubscribeEvents();

        private void SubscribeEvents()
        {
            if (_gameOutcome != null)
                _gameOutcome.GameIsWon += IncreaseUnlockedLevelNumber;
            else
                Debug.LogError("_gameOutcome is null. Cannot subscribe to events.");

            YandexGame.GetDataEvent += GetLoad;
        }

        private void UnsubscribeEvents()
        {
            if (_gameOutcome != null)
                _gameOutcome.GameIsWon -= IncreaseUnlockedLevelNumber;

            YandexGame.GetDataEvent -= GetLoad;
        }

        private void GetLoad()
        {
            UnlockedLevelNumber = YandexGame.savesData.unlockedLevelNumber;
        }

        private void SaveData()
        {
            YandexGame.savesData.unlockedLevelNumber = UnlockedLevelNumber;
            YandexGame.SaveProgress();
            Debug.Log("Progress data saved");
        }

        private void IncreaseUnlockedLevelNumber()
        {
            if (UnlockedLevelNumber == _maxLevelsCount)
                return;

            if (UnlockedLevelNumber > CurrentLevelNumber)
                return;

            UnlockedLevelNumber = CurrentLevelNumber + 1;
            SaveData();
        }
    }
}