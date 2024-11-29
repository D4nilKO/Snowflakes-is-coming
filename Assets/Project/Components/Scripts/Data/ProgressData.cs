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

        public void SetStartedParameters()
        {
            CurrentLevelNumber = 1;
            UnlockedLevelNumber = 1;

            ForceSaveData();
        }

        public void LoadData()
        {
            if (UnlockedLevelNumber >= YandexGame.savesData.unlockedLevelNumber)
                return;

            ForceLoadData();
        }

        public void SaveData()
        {
            if (UnlockedLevelNumber <= YandexGame.savesData.unlockedLevelNumber)
                return;

            ForceSaveData();
        }

        public void ForceSaveData()
        {
            YandexGame.savesData.unlockedLevelNumber = UnlockedLevelNumber;
            YandexGame.SaveProgress();
        }

        public void ForceLoadData()
        {
            UnlockedLevelNumber = YandexGame.savesData.unlockedLevelNumber;
            CurrentLevelNumber = UnlockedLevelNumber;

            Debug.LogWarning("Progress data loaded");
        }

        #endregion

        #region Lifecycle

        private void Awake()
        {
            this.ValidateSerializedFields();

            if (YandexGame.SDKEnabled)
                LoadData();
        }

        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnsubscribeEvents();

        #endregion

        #region Private methods

        private void SubscribeEvents()
        {
            _gameOutcome.GameIsWon += IncreaseUnlockedLevelNumber;
            YandexGame.GetDataEvent += LoadData;
        }

        private void UnsubscribeEvents()
        {
            _gameOutcome.GameIsWon -= IncreaseUnlockedLevelNumber;
            YandexGame.GetDataEvent -= LoadData;
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

        #endregion
    }
}