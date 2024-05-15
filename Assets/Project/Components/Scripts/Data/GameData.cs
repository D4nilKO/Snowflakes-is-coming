using UnityEngine;

namespace Project.Components.Scripts.Data
{
    public static class GameData
    {
        public static int CurrentLevelNumber = 1;
        public static int UnlockedLevelNumber;
        public static int CoinCount;

        public static int MaxLevelsCount;
        
        public static bool IsDataLoaded;
        
        public static bool IsCharacterSpawned;

        public static void IncreaseCurrentLevel()
        {
            if (CurrentLevelNumber != MaxLevelsCount) 
                UnlockedLevelNumber++;
        }

        public static void SaveData() // переписать
        {
            PlayerPrefs.SetInt(nameof(CurrentLevelNumber), CurrentLevelNumber);
            PlayerPrefs.SetInt(nameof(UnlockedLevelNumber), UnlockedLevelNumber);
            PlayerPrefs.SetInt(nameof(CoinCount), CoinCount);
            
            PlayerPrefs.Save();
        }

        public static void LoadData() // переписать
        {
            if (PlayerPrefs.HasKey("CurrentLevelNumber"))
                CurrentLevelNumber = PlayerPrefs.GetInt("CurrentLevelNumber");

            if (PlayerPrefs.HasKey("UnlockedLevelNumber"))
                UnlockedLevelNumber = PlayerPrefs.GetInt("UnlockedLevelNumber");

            if (PlayerPrefs.HasKey("CoinCount"))
                CoinCount = PlayerPrefs.GetInt("CoinCount");
        }

        public static void ClearData()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}