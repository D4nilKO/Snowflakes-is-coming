using Project.Data.Collections;
using UnityEngine;
using VContainer.Unity;
using YG;

namespace Project.Data
{
    public class ProgressData : IStartable
    {
        #region Fields

        private CollectionManager _collectionManager;

        public int MaxLevelsCount { get; private set; }
        public int CurrentLevelNumber { get; private set; } = 1;
        public int UnlockedLevelNumber { get; private set; } = 1;

        #endregion

        public void Start()
        {
            LoadData();
        }

        public bool TryIncreaseCurrentLevel()
        {
            bool notLastLevel = CurrentLevelNumber != MaxLevelsCount;
            bool hasNextLevel = CurrentLevelNumber <= UnlockedLevelNumber;

            if (notLastLevel)
            {
                if (hasNextLevel) CurrentLevelNumber++;
            }
            else
            {
                return false;
            }

            return true;
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
            Debug.Log("ForceSaveData");

            YandexGame.savesData.unlockedLevelNumber = UnlockedLevelNumber;
            YandexGame.SaveProgress();
        }

        public void ForceLoadData()
        {
            UnlockedLevelNumber = YandexGame.savesData.unlockedLevelNumber;
            CurrentLevelNumber = UnlockedLevelNumber;
        }

        public void IncreaseUnlockedLevelNumber()
        {
            if (UnlockedLevelNumber == MaxLevelsCount)
                return;

            if (UnlockedLevelNumber > CurrentLevelNumber)
                return;

            UnlockedLevelNumber = CurrentLevelNumber + 1;
            SaveData();
        }

        public void SetCurrentLevel(int level)
        {
            if (level > MaxLevelsCount && level < 1)
            {
                Debug.LogError("Уровень не существует");
                return;
            }

            if (level > UnlockedLevelNumber)
            {
                Debug.LogError("Уровень не разблокирован");
                return;
            }

            CurrentLevelNumber = level;
        }

        public void SetUnlockedLevel(int level)
        {
            if (level > MaxLevelsCount && level < 1)
            {
                Debug.LogError("Уровень не существует");
                return;
            }

            UnlockedLevelNumber = level;
        }

        public void SetMaxLevelsCount(int maxLevelsCount)
        {
            if (maxLevelsCount <= 0)
            {
                Debug.LogError("Максимальное количество уровней должно быть больше нуля");
                return;
            }

            MaxLevelsCount = maxLevelsCount;
        }

        public void SetStartValues()
        {
            SetUnlockedLevel(1);
            SetCurrentLevel(1);
            ClearUnlockedAbilities();

            YandexGame.savesData.IsSawTraining = false;
            ForceSaveData();
        }

        private void ClearUnlockedAbilities()
        {
            YandexGame.savesData.UnlockedAbilityNames.Clear();
        }
    }
}