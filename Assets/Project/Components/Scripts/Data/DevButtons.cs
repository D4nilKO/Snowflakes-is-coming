using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Components.Scripts.Data
{
    public class DevButtons : MonoBehaviour
    {
        public void InitStartValues()
        {
            GameData.currentLevelNumber = 1;
            GameData.unlockedLevelNumber = 3;
            GameData.coinCount = 0;
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

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
    }
}