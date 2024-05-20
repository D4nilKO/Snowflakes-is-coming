using UnityEngine;

namespace Project.Components.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D),typeof(SpriteRenderer))]
    public abstract class Entity : MonoBehaviour
    {
        protected SpriteRenderer SpriteRenderer;
        protected Camera MainCamera;

        protected static float ScreenWidth;
        protected static float ScreenHeight;
        
        private protected Rigidbody2D Rigidbody2D;
        protected Collider2D ObjectCollider;

        protected float ObjectHeight;
        protected float ObjectWidth;
        
        private float _size;

        public float Size
        {
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
            ObjectCollider = GetComponent<Collider2D>();
            
            MainCamera = Camera.main;
            GetCameraSize(MainCamera);
            Size = gameObject.transform.localScale.x;
        }

        protected void GetObjectSize()
        {
            Bounds bounds = ObjectCollider.bounds;
            
            ObjectWidth = bounds.size.x;
            ObjectHeight = bounds.size.y;
        }

        private static void GetCameraSize(Camera camera)
        {
            ScreenWidth = camera.orthographicSize * camera.aspect * 2f;
            ScreenHeight = camera.orthographicSize * 2f;
        }
    }
}