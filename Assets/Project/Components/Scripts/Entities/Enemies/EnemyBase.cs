using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Project.Components.Scripts.Enemies
{
    public abstract class EnemyBase : Entity
    {
        [Header("Скорость")] [SerializeField] [Range(0f, 20f)]
        protected float speed = 5f;

        [FormerlySerializedAs("rotateEnabled")] [Header("Вращение")] [SerializeField]
        private bool _rotateEnabled;

        [FormerlySerializedAs("rotationSpeed")] [Header("Скорость вращения")] [SerializeField] [Range(1f, 500f)]
        private float _rotationSpeed = 10f;

        private Quaternion targetRotation;

        private protected Vector2 Direction;

        protected virtual void Start()
        {
            GetObjectSize();
            SetRigidbodyVelocity();
            targetRotation = transform.rotation;
        }

        private void OnEnable()
        {
            GetObjectSize();
            SetRigidbodyVelocity();
        }

        public abstract void Move();

        protected void CheckOutOfBounds(ref Vector2 newPosition) // переписать красивее через локальные переменные
        {
            if (newPosition.x < -ScreenWidth / 2f + ObjectWidth / 2f ||
                newPosition.x > ScreenWidth / 2f - ObjectWidth / 2f)
            {
                ReflectHorizontal(ref newPosition);
            }

            if (newPosition.y < -ScreenHeight / 2f + ObjectHeight / 2f ||
                newPosition.y > ScreenHeight / 2f - ObjectHeight / 2f)
            {
                ReflectVertical(ref newPosition);
            }
        }

        protected virtual void ReflectHorizontal(ref Vector2 position)
        {
            Direction = new Vector2(-Direction.x, Direction.y);
            Bounds bounds = objectCollider.bounds;
            position.x = Mathf.Clamp(position.x, -ScreenWidth / 2f + bounds.extents.x,
                ScreenWidth / 2f - bounds.extents.x);
        }

        protected virtual void ReflectVertical(ref Vector2 position)
        {
            Direction = new Vector2(Direction.x, -Direction.y);
            Bounds bounds = objectCollider.bounds;
            position.y = Mathf.Clamp(position.y, -ScreenHeight / 2f + bounds.extents.y,
                ScreenHeight / 2f - bounds.extents.y);
        }

        public virtual void Rotate()
        {
            if (_rotateEnabled == false) return;
            float newRotation = targetRotation.eulerAngles.z + (_rotationSpeed * Time.fixedDeltaTime);

            targetRotation = Quaternion.Euler(0f, 0f, newRotation);

            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        }

        protected virtual void SetRigidbodyVelocity()
        {
            Rigidbody2D.velocity = Direction * speed;
        }

        protected Vector2 GetRandomDirection()
        {
            return Random.insideUnitCircle.normalized;
        }
    }
}