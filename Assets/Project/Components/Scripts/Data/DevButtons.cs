using UnityEngine;

namespace Project.Components.Scripts.Data
{
    public class DevButtons : MonoBehaviour
    {
        public void InitStartValues()
        {
            GameData.currentLevelNumber = 1;
            GameData.unlockedLevelNumber = 3;
            GameData.coinCount = 100;
            GameData.SaveData();
        }

        public void SavePlayerPrefs()
        {
            GameData.SaveData();
        }

        public void ClearPlayerPrefs()
        {
            GameData.ClearData();
        }

        public void LoadPlayerPrefs()
        {
            GameData.LoadData();
        }
    }
}