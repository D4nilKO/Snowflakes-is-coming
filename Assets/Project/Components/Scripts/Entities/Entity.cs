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
        
        private protected Rigidbody2D Rigidbody2D;
        protected Collider2D objectCollider;

        protected float ObjectHeight;
        protected float ObjectWidth;
        
        private float _size;

        public float Size
        {
            get => _size;
            set
            {
                gameObject.transform.localScale = new Vector3(value, value, 1f);
                _size = value;
                GetObjectSize();
            }
        }

        public virtual void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            objectCollider = GetComponent<Collider2D>();
            mainCamera = Camera.main;
            GetCameraSize(mainCamera);
            Size = gameObject.transform.localScale.x;
        }

        protected void GetObjectSize()
        {
            Bounds bounds = objectCollider.bounds;
            ObjectWidth = bounds.size.x;
            ObjectHeight = bounds.size.y;
        }

        private static void GetCameraSize(Camera camera)
        {
            ScreenWidth = camera.orthographicSize * camera.aspect * 2f; // Получение ширины экрана
            ScreenHeight = camera.orthographicSize * 2f; // Получение высоты экрана
        }
    }
}