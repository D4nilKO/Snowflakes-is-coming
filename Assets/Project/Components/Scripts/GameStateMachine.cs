using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Components.Scripts
{
    public class GameStateMachine : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverCanvas;
        [SerializeField] private GameObject wonLevelCanvas;
        //private bool gameIsWon;


        private TimeManager timeManager;

        private bool gamePaused;

        private void Awake()
        {
            timeManager = GetComponent<TimeManager>();
        }

        public void LostGame()
        {
            PauseGame(gameOverCanvas);
        }

        public void ResumeGame()
        {
            if (!gamePaused) return;

            gamePaused = false;
            timeManager.ApplyWaitBeforeContinueTime();

            gamePaused = false;
        }

        void PauseGame(GameObject canvasToSetActive)
        {
            Time.timeScale = 0f;
            canvasToSetActive.SetActive(true);
            gamePaused = true;
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }

        public void WonLevel()
        {
            //gameIsWon = true;
            PauseGame(wonLevelCanvas);
        }
    }
}