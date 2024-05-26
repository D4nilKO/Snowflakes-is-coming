// using UnityEngine;
// using UnityEngine.SceneManagement;
//
// namespace Project.Components.Scripts.Data
// {
//     public class DevButtons : MonoBehaviour
//     {
//         public void InitStartValues()
//         {
//             ProgressData.CurrentLevelNumber = 1;
//             ProgressData.UnlockedLevelNumber = 3;
//             ProgressData.CoinCount = 0;
//             ProgressData.SaveData();
//         }
//
//         public void SavePlayerPrefs()
//         {
//             ProgressData.SaveData();
//         }
//
//         public void ClearPlayerPrefs()
//         {
//             ProgressData.ClearData();
//         }
//
//         public void LoadPlayerPrefs()
//         {
//             ProgressData.LoadData();
//         }
//
//         public void RestartLevel()
//         {
//             SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
//         }
//     }
// }