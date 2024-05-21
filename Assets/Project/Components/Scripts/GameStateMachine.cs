using UnityEngine;
using UnityEngine.SceneManagement;
using static Project.Components.Scripts.Data.GameData;

namespace Project.Components.Scripts
{
    [RequireComponent(typeof(TimeManager))]
    public class GameStateMachine : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverCanvas;
        [SerializeField] private GameObject _wonLevelCanvas;

        private bool _isWonGame;

        private TimeManager _timeManager;

        private static bool s_gamePaused;

        private void Awake()
        {
            _timeManager = GetComponent<TimeManager>();
            FirstLoadData();
        }

        private void FirstLoadData()
        {
            if (IsDataLoaded == false)
            {
                LoadData();
                IsDataLoaded = true;
            }

            s_gamePaused = true;
        }

        public void LostGame()
        {
            PauseGame(_gameOverCanvas);
        }

        public void ResumeGame(GameObject canvasToSetActive)
        {
            if (s_gamePaused == false) 
                return;

            canvasToSetActive.SetActive(false);
            _timeManager.ApplyWaitBeforeContinueTime();

            s_gamePaused = true;
        }

        public void ResumeGame()
        {
            if (s_gamePaused == false) 
                return;

            _timeManager.ApplyWaitBeforeContinueTime();
            s_gamePaused = true;
        }

        private void PauseGame(GameObject canvasToSetActive)
        {
            canvasToSetActive.SetActive(true);
            Time.timeScale = 0f;
            s_gamePaused = true;
        }

        public void PauseGame()
        {
            Time.timeScale = 0f;
            s_gamePaused = true;
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single); // переписать
        }

        public void WonLevel()
        {
            _isWonGame = true;
            PauseGame(_wonLevelCanvas);

            IncreaseCurrentLevel();
        }

        public void NextLevel()
        {
            if (CurrentLevelNumber != MaxLevelsCount)
                CurrentLevelNumber++;
            else
                CurrentLevelNumber = 1;

            RestartLevel();
        }
        
        private void OnApplicationQuit()
        {
            if (Application.isEditor == false)
            {
                SaveData();
            }
        }
    }
}