using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Components.Scripts.Enemies
{
    public abstract class EnemyBase : Entity
    {
        [Header("Скорость")] [SerializeField] [Range(0f, 20f)]
        protected float speed = 5f;

        [Header("Вращение")] [SerializeField] 
        private bool rotateEnabled;

        [Header("Скорость вращения")] [SerializeField] [Range(1f, 500f)]
        private float rotationSpeed = 10f;

        private Quaternion targetRotation;

        private protected Vector2 Direction;

        protected virtual void Start()
        {
            TakeObjectSize();
            SetRbVelocity();
            targetRotation = transform.rotation;
        }

        private void OnEnable()
        {
            TakeObjectSize();
            SetRbVelocity();
        }

        public abstract void Move();
        
        protected void CheckOutOfBounds(ref Vector2 _newPosition)
        {
            if (_newPosition.x < -ScreenWidth / 2f + ObjectWidth / 2f ||
                _newPosition.x > ScreenWidth / 2f - ObjectWidth / 2f)
            {
                ReflectHorizontal(ref _newPosition);
            }
        
            if (_newPosition.y < -ScreenHeight / 2f + ObjectHeight / 2f ||
                _newPosition.y > ScreenHeight / 2f - ObjectHeight / 2f)
            {
                ReflectVertical(ref _newPosition);
            }
        }

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

        protected virtual void SetRbVelocity()
        {
            Rb2D.velocity = Direction * speed; // Установка начальной скорости
        }

        protected Vector2 GetRandomDirection()
        {
            return Random.insideUnitCircle.normalized;
        }
    }
}