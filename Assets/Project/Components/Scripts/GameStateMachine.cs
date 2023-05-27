using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Components.Scripts
{
    public class GameStateMachine : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverCanvas;
        [SerializeField] private GameObject wonLevelCanvas;
        private bool gameIsWon;


        private TimeManager timeManager;

        private bool gamePaused;

        public void LostGame()
        {
            PauseGame(gameOverCanvas);
        }

        public void ResumeGame(GameObject canvasToSetActive)
        {
            if (!gamePaused) return;

            timeManager.ApplyWaitBeforeContinueTime();
            canvasToSetActive.SetActive(true);
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
            Time.timeScale = 1f;
        }

        public void WonLevel()
        {
            gameIsWon = true;
            PauseGame(wonLevelCanvas);
        }
    }
}