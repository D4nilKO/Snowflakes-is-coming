using Project.Architecture.UI;
using UnityEngine;
using VContainer;
using YG;

namespace Project.Timing
{
    public class PauseHandler
    {
        #region Fields

        private InGamePausePanel _inGamePauseCanvas;

        private float _startTimeScale = 1;

        #endregion

        [Inject]
        public void Construct(InGamePausePanel inGamePausePanel)
        {
            _inGamePauseCanvas = inGamePausePanel;
            inGamePausePanel.AddListeners(this);
        }

        #region Public methods

        public void Resume()
        {
            Time.timeScale = _startTimeScale;
            YandexGame.GameplayStart();
        }

        public void Resume(GameObject canvasToSetActive)
        {
            canvasToSetActive.SetActive(false);
            Resume();
        }

        public void Pause()
        {
            Time.timeScale = 0f;
            YandexGame.GameplayStop();
        }

        public void Pause(GameObject gameObjectToSetActive)
        {
            gameObjectToSetActive.SetActive(true);

            Pause();
        }

        public void InGamePause()
        {
            Pause(_inGamePauseCanvas.gameObject);
        }

        public void InGameResume()
        {
            Resume(_inGamePauseCanvas.gameObject);
        }

        #endregion
    }
}