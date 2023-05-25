using UnityEngine;

namespace Project.Components.Scripts
{
    public class TimeHandler
    {
        
        private bool gamePaused = false;
        
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
                //canvasPanel.SetActive(false); // Скрываем панель на canvas
                gamePaused = false;
            }
        }
    }
}