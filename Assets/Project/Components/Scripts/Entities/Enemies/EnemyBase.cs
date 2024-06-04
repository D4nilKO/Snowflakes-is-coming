using NTC.Global.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Components.Scripts.Entities.Enemies
{
    public abstract class EnemyBase : Entity, IMovable, IPoolItem
    {
        [SerializeField] [Range(0f, 20f)] private float _speed = 5f;

        [SerializeField] [Range(1f, 359f)] private float _rotationSpeed = 10f;
        [SerializeField] private bool _rotateEnabled;

        private Quaternion _targetRotation;

        private Vector2 _direction;
        protected float Speed => _speed;

        protected Vector2 Direction
        {
            get => _direction;
            private set
            {
                if (value == Vector2.zero)
                    return;

                _direction = value.normalized;
            }
        }

        public abstract void Move();
        public abstract void OnSpawn();
        public abstract void OnDespawn();
        
        public void Rotate()
        {
            if (_rotateEnabled == false)
                return;

            float newRotation = _targetRotation.eulerAngles.z + (_rotationSpeed * Time.deltaTime);

            _targetRotation = Quaternion.Euler(0f, 0f, newRotation);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                _targetRotation,
                _rotationSpeed * Time.deltaTime);
        }

        protected virtual void Start()
        {
            _targetRotation = transform.rotation;
        }

        protected void SetRandomDirection()
        {
            Direction = Random.insideUnitCircle.normalized;
        }

        protected void CheckOutOfBounds(ref Vector2 newPosition)
        {
            float halfScreenWidth = ScreenWidth / 2f;
            float halfScreenHeight = ScreenHeight / 2f;

            float halfObjectWidth = ObjectWidth / 2f;
            float halfObjectHeight = ObjectHeight / 2f;

            bool isOutOfBoundsX = newPosition.x < -halfScreenWidth + halfObjectWidth ||
                                  newPosition.x > halfScreenWidth - halfObjectWidth;

            bool isOutOfBoundsY = newPosition.y < -halfScreenHeight + halfObjectHeight ||
                                  newPosition.y > halfScreenHeight - halfObjectHeight;

            if (isOutOfBoundsX)
                ReflectHorizontal(ref newPosition);

            if (isOutOfBoundsY)
                ReflectVertical(ref newPosition);
        }

        private void ReflectHorizontal(ref Vector2 position)
        {
            Direction = new Vector2(-Direction.x, Direction.y);
            Bounds bounds = ObjectCollider.bounds;
            position.x = Mathf.Clamp(
                position.x,
                (-ScreenWidth / 2f) + bounds.extents.x,
                (ScreenWidth / 2f) - bounds.extents.x);
        }

        private void ReflectVertical(ref Vector2 position)
        {
            Direction = new Vector2(Direction.x, -Direction.y);
            Bounds bounds = ObjectCollider.bounds;
            position.y = Mathf.Clamp(
                position.y,
                (-ScreenHeight / 2f) + bounds.extents.y,
                (ScreenHeight / 2f) - bounds.extents.y);
        }

        private void OnEnable()
        {
            UpdateBoundsValue();
            SetRigidbodyVelocity();
        }

        private void SetRigidbodyVelocity()
        {
            Rigidbody2D.velocity = Direction * _speed;
        }
    }
}