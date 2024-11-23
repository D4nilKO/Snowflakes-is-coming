using UnityEngine;
using DG.Tweening;

namespace Project.VisualEffects
{
    public class PulsationEffect : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 1f)]
        private float _pulsationDuration = 0.3f;

        [SerializeField, Range(1.01f, 5f)]
        private float _endValue = 1.2f;

        [SerializeField]
        private bool _isPulsating = true;

        private void OnEnable()
        {
            TogglePulsation(_isPulsating);
        }

        private void OnDisable()
        {
            StopPulsation();
        }

        private void OnValidate()
        {
            TogglePulsation(_isPulsating);
        }

        private void TogglePulsation(bool pulsate)
        {
            if (pulsate)
                StartPulsation();
            else
                StopPulsation();
        }

        private void StartPulsation()
        {
            StopPulsation();
            _isPulsating = true;
            transform.DOScale(_endValue, _pulsationDuration).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
        }

        private void StopPulsation()
        {
            transform.DOKill();
            transform.localScale = Vector3.one;
        }
    }
}