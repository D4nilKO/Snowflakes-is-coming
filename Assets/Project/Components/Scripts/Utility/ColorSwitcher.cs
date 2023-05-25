using UnityEngine;

namespace Project.Components.Scripts.Utility
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class ColorSwitcher: MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        
        [Header("Цвет")] [SerializeField] [ColorUsage(true, true)]
        private Color currentColor;

        public Color CurrentColor
        {
            get => currentColor;
            set
            {
                currentColor = value;
                ColorSwitch();
            }
        }

        private void ColorSwitch()
        {
            _spriteRenderer.color = CurrentColor;
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            ColorSwitch();
        }
        
#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(this)) return;
            if (_spriteRenderer.color != CurrentColor)
            {
                ColorSwitch();
            }
        }
#endif
        
    }
}