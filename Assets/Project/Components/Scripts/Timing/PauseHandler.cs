using System;
using System.Collections;
using UnityEngine;

namespace Project.Components.Scripts.Timing
{
    public class PauseHandler : MonoBehaviour
    {
        [SerializeField] private float _startTimeScale = 1;
        [SerializeField] private float _startTimePauseBeforeContinueTime = 0.5f;
        
        private static bool s_gamePaused;
        
        private Coroutine _currentCoroutine;

        private void Awake()
        {
            Time.timeScale = _startTimeScale;
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

        public void Pause()
        {
            Time.timeScale = 0f;
            s_gamePaused = true;
        }

        public void ApplyWaitBeforeContinueTime()
        {
            if (_currentCoroutine != null) 
                StopCoroutine(_currentCoroutine);
            
            _currentCoroutine = StartCoroutine(UnscaledWaitBeforeContinueTime());
        }

        public void Resume(GameObject canvasToSetActive)
        {
            if (s_gamePaused == false) 
                return;

            canvasToSetActive.SetActive(false);
            ApplyWaitBeforeContinueTime();

            s_gamePaused = true;
        }

        public void Resume()
        {
            if (s_gamePaused == false) 
                return;

            ApplyWaitBeforeContinueTime();
            s_gamePaused = true;
        }
    }
}