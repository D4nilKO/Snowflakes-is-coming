using Project.Architecture.UI;
using UnityEngine;
using VContainer;
using YG;

namespace Project.Timing
{
    public class PauseHandler
    {
        #region Fields

        private float _startTimeScale = 1;
        private GameObject _inGamePauseCanvas;

        private static bool GamePaused { get; set; }

        #endregion
        
        [Inject]
        public void Construct(InGamePausePanel inGamePausePanel)
        {
            _startTimeScale = Time.timeScale;
            _inGamePauseCanvas = inGamePausePanel.gameObject;
        }

        #region Public methods

        public void Resume()
        {
            if (GamePaused == false)
                return;

            Time.timeScale = _startTimeScale;
            YandexGame.GameplayStart();

            GamePaused = true;
        }

        public void Resume(GameObject canvasToSetActive)
        {
            if (GamePaused == false)
                return;

            canvasToSetActive.SetActive(false);
            Resume();
        }

        public static void Pause()
        {
            Time.timeScale = 0f;
            GamePaused = true;

            YandexGame.GameplayStop();
        }

        public static void Pause(GameObject gameObjectToSetActive)
        {
            gameObjectToSetActive.SetActive(true);
            Time.timeScale = 0f;
            GamePaused = true;
        }

        public void ForceInGamePause()
        {
            Pause(_inGamePauseCanvas);
        }

        public void InGamePause()
        {
            if (GamePaused)
                return;

            Pause(_inGamePauseCanvas);
        }

        public void InGameResume()
        {
            if (GamePaused == false)
                return;

            Resume(_inGamePauseCanvas);
        }

        #endregion
    }
}