using UnityEngine;

namespace Project.Utility
{
    [ExecuteAlways] 
    [DisallowMultipleComponent]
    public class ColorSwitcher : MonoBehaviour
    {
        // Подправить
        private SpriteRenderer _spriteRenderer;

        [Header("Цвет")] [SerializeField] [ColorUsage(true, true)]
        private Color _currentColor;

        public Color CurrentColor
        {
            get => _currentColor;
            set
            {
                _currentColor = value;
                ColorSwitch();
            }
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            ColorSwitch();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(this))
                return;

            if (_spriteRenderer.color != CurrentColor)
                ColorSwitch();
        }

#endif

        private void ColorSwitch()
        {
            _spriteRenderer.color = CurrentColor;
        }
    }
}