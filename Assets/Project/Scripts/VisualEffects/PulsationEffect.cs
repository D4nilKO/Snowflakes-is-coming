using UnityEngine;
using DG.Tweening;

namespace Project.VisualEffects
{
    [DisallowMultipleComponent]
    public class PulsationEffect : MonoBehaviour
    {
        #region Fields

        [Header("Pulsation Settings")]
        [SerializeField, Range(0.1f, 1f)]
        private float _pulsationDuration = 0.3f;

        [SerializeField, Range(1.01f, 5f)]
        private float _endValue = 1.2f;

        [SerializeField]
        private bool _isPulsating = true;

        [SerializeField]
        private bool _resetScaleOnStop = true;

        private Tween _pulsationTween;

        #endregion

        #region Lifecycle

        private void OnEnable()
        {
            if (_isPulsating)
            {
                StartPulsation();
            }
        }

        private void OnDisable()
        {
            StopPulsation();
        }

        private void OnValidate()
        {
            if (Application.isPlaying == false)
                return;

            TogglePulsation(_isPulsating);
        }

        #endregion

        #region Private Methods

        private void TogglePulsation(bool pulsate)
        {
            _isPulsating = pulsate;

            if (_isPulsating)
            {
                StartPulsation();
            }
            else
            {
                StopPulsation();
            }
        }

        private void StartPulsation()
        {
            if (gameObject.activeInHierarchy == false)
                return;

            StopPulsation();

            _pulsationTween = transform.DOScale(_endValue, _pulsationDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(true);
        }

        private void StopPulsation()
        {
            if (_pulsationTween != null && _pulsationTween.IsActive())
            {
                _pulsationTween.Kill();
            }

            ResetScale();
        }

        private void ResetScale()
        {
            if (_resetScaleOnStop)
            {
                transform.localScale = Vector3.one;
            }
        }

        #endregion
    }
}