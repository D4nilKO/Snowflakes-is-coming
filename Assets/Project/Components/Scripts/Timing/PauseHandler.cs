using System.Collections;
using UnityEngine;

namespace Project.Timing
{
    public class PauseHandler : MonoBehaviour
    {
        [SerializeField]
        private float _startTimeScale = 1;

        [SerializeField]
        private float _startTimePauseBeforeContinueTime = 0.5f;

        private static bool s_gamePaused;

        private Coroutine _currentCoroutine;

        public void Resume(GameObject canvasToSetActive)
        {
            if (s_gamePaused == false)
                return;

            canvasToSetActive.SetActive(false);
            Resume();
        }

        public void Resume()
        {
            if (s_gamePaused == false)
                return;

            ApplyWaitBeforeContinueTime(); // Закоментировать следующую строку, если текущая строка расскоментирована.
            // Time.timeScale = _startTimeScale;

            s_gamePaused = true;
        }

        public void Pause()
        {
            Time.timeScale = 0f;
            s_gamePaused = true;
        }

        private void Pause(GameObject canvasToSetActive)
        {
            canvasToSetActive.SetActive(true);
            Time.timeScale = 0f;
            s_gamePaused = true;
        }

        private IEnumerator UnscaledWaitBeforeContinueTime()
        {
            float timePauseBeforeContinueTime = _startTimePauseBeforeContinueTime;

            while (timePauseBeforeContinueTime > 0)
            {
                timePauseBeforeContinueTime -= Time.unscaledDeltaTime;
                yield return null;
            }

            Time.timeScale = _startTimeScale;
        }

        private void ApplyWaitBeforeContinueTime()
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);

            _currentCoroutine = StartCoroutine(UnscaledWaitBeforeContinueTime());
        }
    }
}