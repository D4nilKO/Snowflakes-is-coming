using UnityEngine;
using YG;

namespace Project.Data
{
    static class ProgressData
    {
        #region Fields

        public static int MaxLevelsCount { get; private set; }
        public static int CurrentLevelNumber { get; private set; } = 1;
        public static int UnlockedLevelNumber { get; private set; } = 1;

        #endregion

        #region Constructors

        static ProgressData()
        {
            if (YandexGame.SDKEnabled)
                LoadData();
            
            
        }

        #endregion

        #region Public methods

        public static bool TryIncreaseCurrentLevel()
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

        public static void SetMaxLevelsCount(int maxLevelsCount)
        {
            if (maxLevelsCount <= 0)
            {
                Debug.LogError("Максимальное количество уровней должно быть больше нуля");
                return;
            }

            MaxLevelsCount = maxLevelsCount;
        }

        public static void LoadData()
        {
            if (UnlockedLevelNumber >= YandexGame.savesData.unlockedLevelNumber)
                return;

            ForceLoadData();
        }

        public static void SaveData()
        {
            if (UnlockedLevelNumber <= YandexGame.savesData.unlockedLevelNumber)
                return;

            ForceSaveData();
        }

        public static void ForceSaveData()
        {
            YandexGame.savesData.unlockedLevelNumber = UnlockedLevelNumber;
            YandexGame.SaveProgress();
        }

        public static void ForceLoadData()
        {
            UnlockedLevelNumber = YandexGame.savesData.unlockedLevelNumber;
            CurrentLevelNumber = UnlockedLevelNumber;
        }

        public static void IncreaseUnlockedLevelNumber()
        {
            if (UnlockedLevelNumber == MaxLevelsCount)
                return;

            if (UnlockedLevelNumber > CurrentLevelNumber)
                return;

            UnlockedLevelNumber = CurrentLevelNumber + 1;
            SaveData();
        }

        #endregion

        public static void SetCurrentLevel(int level)
        {
            if (level > MaxLevelsCount)
            {
                Debug.LogError("Уровень не существует");
                return;
            }
            if (level < 1)
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
    }
}