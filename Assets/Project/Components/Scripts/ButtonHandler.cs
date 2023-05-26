using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Components.Scripts
{
    public class ButtonHandler : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverCanvas;
        private bool gamePaused;

        public void LostGame()
        {
            PauseGame(gameOverCanvas);
        }

        public void PauseGame(GameObject canvasToSetActive)
        {
            Time.timeScale = 0f;
            canvasToSetActive.SetActive(true);
            gamePaused = true;
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex,LoadSceneMode.Single);
            Time.timeScale = 1f;
        }
    }
}