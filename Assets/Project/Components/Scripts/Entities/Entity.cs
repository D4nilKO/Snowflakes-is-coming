using UnityEngine;

namespace Project.Entities
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public abstract class Entity : MonoBehaviour
    {
        protected Rigidbody2D Rigidbody2D;

        protected Camera MainCamera;

        protected Collider2D ObjectCollider;

        protected float ScreenWidth;
        protected float ScreenHeight;

        protected float ObjectHeight;
        protected float ObjectWidth;

        private float _size;

        public float Size
        {
            get => _size;
            set
            {
                if (value <= 0f)
                {
                    Debug.LogError("Entity size must be greater than zero.");
                    return;
                }

                gameObject.transform.localScale = new Vector3(value, value, 1f);
                _size = value;

                UpdateBoundsValue();
            }
        }

        public virtual void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            ObjectCollider = GetComponent<Collider2D>();

            MainCamera = Camera.main;
            FetchCameraSize(MainCamera, out ScreenWidth, out ScreenHeight);

            Size = gameObject.transform.localScale.x;
        }

        protected void UpdateBoundsValue()
        {
            Bounds bounds = ObjectCollider.bounds;
            ObjectWidth = bounds.size.x;
            ObjectHeight = bounds.size.y;
        }

        private static void FetchCameraSize(Camera camera, out float screenWidth, out float screenHeight)
        {
            screenWidth = camera.orthographicSize * camera.aspect * 2f;
            screenHeight = camera.orthographicSize * 2f;
        }
    }
}