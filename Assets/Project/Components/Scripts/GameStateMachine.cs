using Project.Components.Scripts.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Project.Components.Scripts.Data.GameData;

namespace Project.Components.Scripts
{
    public class GameStateMachine : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverCanvas;
        [SerializeField] private GameObject wonLevelCanvas;
        private bool gameIsWon;

        private TimeManager timeManager;

        public static bool GamePaused;

        private void Awake()
        {
            timeManager = GetComponent<TimeManager>();
            FirstLoadData();
        }

        private void FirstLoadData()
        {
            if (!dataIsLoaded)
            {
                LoadData();
                dataIsLoaded = true;
            }

            GamePaused = true;
        }

        public void LostGame()
        {
            PauseGame(gameOverCanvas);
        }

        public void ResumeGame(GameObject canvasToSetActive)
        {
            if (!GamePaused) return;

            canvasToSetActive.SetActive(false);

            timeManager.ApplyWaitBeforeContinueTime();

            GamePaused = true;
        }

        public void ResumeGame()
        {
            if (!GamePaused) return;

            timeManager.ApplyWaitBeforeContinueTime();

            GamePaused = true;
        }

        private void PauseGame(GameObject canvasToSetActive)
        {
            Time.timeScale = 0f;
            canvasToSetActive.SetActive(true);
            GamePaused = true;
        }

        public void PauseGame()
        {
            Time.timeScale = 0f;
            GamePaused = true;
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }

        public void WonLevel()
        {
            gameIsWon = true;
            PauseGame(wonLevelCanvas);

            GameData.IncreaseCurrentLevel();
        }

        public void NextLevel()
        {
            if (currentLevelNumber != maxLevelsCount)
            {
                currentLevelNumber++;
            }
            else
            {
                currentLevelNumber = 1;
            }

            RestartLevel();
        }

        // Расскоментить для релиза
        // private void OnApplicationQuit()
        // {
        //     SaveData();
        // }
    }
}