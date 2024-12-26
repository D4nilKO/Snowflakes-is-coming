using DG.Tweening;
using UnityEngine;

namespace Project.VisualEffects
{
    public class FloatingObject : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Tweener _tween;

        private float _jumpHeight = 20f;
        private float _duration = 0.5f;
        private float _delay = 0f;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            _tween?.Play();
        }

        private void OnDisable()
        {
            _tween?.Pause();
        }

        public void Initialize(float jumpHeight, float duration, float delay)
        {
            _jumpHeight = jumpHeight;
            _duration = duration;
            _delay = delay;

            StartFloating();
        }

        private void StartFloating()
        {
            _tween = _rectTransform.DOAnchorPosY(_jumpHeight, _duration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad)
                .SetDelay(_delay)
                .SetUpdate(true);
        }
    }
}