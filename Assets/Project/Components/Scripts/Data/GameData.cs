using UnityEngine;

namespace Project.Components.Scripts.Data
{
    public static class GameData
    {
        public static int currentLevelNumber = 1;
        public static int unlockedLevelNumber;
        public static int coinCount;
        
        public static bool characterIsSpawned;

        public static void SaveData()
        {
            PlayerPrefs.SetInt("CurrentLevelNumber", currentLevelNumber);
            PlayerPrefs.SetInt("UnlockedLevelNumber", unlockedLevelNumber);
            PlayerPrefs.SetInt("CoinCount", coinCount);
            PlayerPrefs.Save();
        }

        public static void LoadData()
        {
            if (PlayerPrefs.HasKey("CurrentLevelNumber"))
                currentLevelNumber = PlayerPrefs.GetInt("CurrentLevelNumber");

            if (PlayerPrefs.HasKey("UnlockedLevelNumber"))
                unlockedLevelNumber = PlayerPrefs.GetInt("UnlockedLevelNumber");

            if (PlayerPrefs.HasKey("CoinCount"))
                coinCount = PlayerPrefs.GetInt("CoinCount");
        }

        public static void ClearData()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}