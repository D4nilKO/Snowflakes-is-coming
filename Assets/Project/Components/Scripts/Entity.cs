using UnityEngine;

namespace Project.Components.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D),typeof(SpriteRenderer))]
    public abstract class Entity : MonoBehaviour
    {
        protected SpriteRenderer spriteRenderer;
        protected Camera mainCamera;

        protected static float ScreenWidth;
        protected static float ScreenHeight;
        
        private protected Rigidbody2D Rb2D;
        protected Collider2D objectCollider;

        protected float ObjectHeight;
        protected float ObjectWidth;

        [SerializeField] protected float standardSize = 0.2f;
        private float _size;

        public float Size
        {
            get => _size;
            set
            {
                gameObject.transform.localScale = new Vector3(value, value, 1f);
                _size = value;
                TakeObjectSize();
            }
        }
        
        protected virtual void Awake()
        {
            Rb2D = GetComponent<Rigidbody2D>();
            objectCollider = GetComponent<Collider2D>();
            mainCamera = Camera.main;
            TakeCameraSize(mainCamera);
            Size = standardSize;
        }

        protected void TakeObjectSize()
        {
            var bounds = objectCollider.bounds;
            ObjectWidth = bounds.size.x;
            ObjectHeight = bounds.size.y;
        }
        
        public static void TakeCameraSize(Camera _camera)
        {
            ScreenWidth = _camera.orthographicSize * _camera.aspect * 2f; // Получение ширины экрана
            ScreenHeight = _camera.orthographicSize * 2f; // Получение высоты экрана
        }
    }
}