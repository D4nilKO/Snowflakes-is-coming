using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Components.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public abstract class EnemyBase : MonoBehaviour
    {
        [Header("Скорость")] [SerializeField] [Range(0f, 20f)]
        protected float speed = 5f;

        [Header("Вращение")] [SerializeField] 
        private bool rotateEnabled;

        [Header("Скорость вращения")] [SerializeField] [Range(1f, 500f)]
        private float rotationSpeed = 10f;

        private Quaternion targetRotation;

        private const float StandardSize = 0.2f;
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

        private protected Vector2 Direction;
        private protected Rigidbody2D Rb2D;
        private Collider2D objectCollider;

        protected static float ScreenWidth;
        protected static float ScreenHeight;

        private protected float ObjectHeight;
        private protected float ObjectWidth;

        protected virtual void Awake()
        {
            Rb2D = GetComponent<Rigidbody2D>();
            objectCollider = GetComponent<Collider2D>();
            Size = StandardSize;
        }

        protected virtual void Start()
        {
            TakeObjectSize();
            UpdateRbVelocity();
            targetRotation = transform.rotation;
        }

        private void OnEnable()
        {
            TakeObjectSize();
            UpdateRbVelocity();
        }

        public abstract void Move();

        protected virtual void ReflectHorizontal(ref Vector2 position)
        {
            Direction = new Vector2(-Direction.x, Direction.y);
            var bounds = objectCollider.bounds;
            position.x = Mathf.Clamp(position.x, -ScreenWidth / 2f + bounds.extents.x,
                ScreenWidth / 2f - bounds.extents.x);
        }

        protected virtual void ReflectVertical(ref Vector2 position)
        {
            Direction = new Vector2(Direction.x, -Direction.y);
            var bounds = objectCollider.bounds;
            position.y = Mathf.Clamp(position.y, -ScreenHeight / 2f + bounds.extents.y,
                ScreenHeight / 2f - bounds.extents.y);
        }
        
        public virtual void Rotate()
        {
            if (!rotateEnabled) return;
            var newRotation = targetRotation.eulerAngles.z + (rotationSpeed * Time.fixedDeltaTime);

            targetRotation = Quaternion.Euler(0f, 0f, newRotation);

            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        protected virtual void UpdateRbVelocity()
        {
            Rb2D.velocity = Direction * speed; // Установка начальной скорости
        }

        private void TakeObjectSize()
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

        protected Vector2 GetRandomDirection()
        {
            return Random.insideUnitCircle.normalized;
        }
    }
}