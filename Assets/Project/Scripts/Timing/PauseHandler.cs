using System.Collections;
using UnityEngine;
using YG;

namespace Project.Timing
{
    public class PauseHandler : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private float _startTimeScale = 1;

        [SerializeField]
        private float _startTimePauseBeforeContinueTime = 0.5f;

        [SerializeField]
        private GameObject _inGamePauseCanvas;

        private Coroutine _currentCoroutine;

        public static bool GamePaused { get; private set; }

        #endregion

        #region Private methods

        private IEnumerator UnscaledWaitBeforeContinueTime()
        {
            float timePauseBeforeContinueTime = _startTimePauseBeforeContinueTime;

            while (timePauseBeforeContinueTime > 0)
            {
                timePauseBeforeContinueTime -= Time.unscaledDeltaTime;
                yield return null;
            }

            Time.timeScale = _startTimeScale;
            YandexGame.GameplayStart();
        }

        private void ApplyWaitBeforeContinueTime()
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);

            _currentCoroutine = StartCoroutine(UnscaledWaitBeforeContinueTime());
        }

        #endregion

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

        public static void Pause(GameObject canvasToSetActive)
        {
            canvasToSetActive.SetActive(true);
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