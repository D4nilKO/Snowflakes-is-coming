using UnityEngine;

namespace Project.Components.Scripts
{
    public class CollisionHandler : MonoBehaviour
    {
        public GameObject canvasPanel;
    
        private bool gamePaused = false;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<Rigidbody2D>() != null && collision.gameObject.GetComponent<Collider2D>().isTrigger)
            {
                ShowCanvasPanel();
                PauseGame();
            }
        }

        private void ShowCanvasPanel()
        {
            canvasPanel.SetActive(true); // Показываем панель на canvas
        }

        private void PauseGame()
        {
            Time.timeScale = 0f; // Останавливаем игру
            gamePaused = true;
        }

        public void ResumeGame()
        {
            if (gamePaused)
            {
                Time.timeScale = 1f; // Возобновляем игру
                canvasPanel.SetActive(false); // Скрываем панель на canvas
                gamePaused = false;
            }
        }
    }
}