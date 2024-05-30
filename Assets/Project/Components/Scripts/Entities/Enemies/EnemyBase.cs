using NTC.Global.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Components.Scripts.Entities.Enemies
{
    public abstract class EnemyBase : Entity, IMovable, IPoolItem
    {
        [SerializeField] [Range(0f, 20f)] protected float _speed = 5f;

        [SerializeField] private bool _rotateEnabled;

        [SerializeField] [Range(1f, 359f)] private float _rotationSpeed = 10f;

        private Quaternion _targetRotation;
        
        // сделать условие в свойстве
        protected Vector2 Direction { get; private set; }

        public abstract void Move();

        protected virtual void Start()
        {
            _targetRotation = transform.rotation;
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

        protected virtual void ReflectHorizontal(ref Vector2 position)
        {
            Direction = new Vector2(-Direction.x, Direction.y);
            Bounds bounds = ObjectCollider.bounds;
            position.x = Mathf.Clamp(position.x, -ScreenWidth / 2f + bounds.extents.x,
                ScreenWidth / 2f - bounds.extents.x);
        }

        protected virtual void ReflectVertical(ref Vector2 position)
        {
            Direction = new Vector2(Direction.x, -Direction.y);
            Bounds bounds = ObjectCollider.bounds;
            position.y = Mathf.Clamp(position.y, -ScreenHeight / 2f + bounds.extents.y,
                ScreenHeight / 2f - bounds.extents.y);
        }

        protected void SetRandomDirection()
        {
            Direction = Random.insideUnitCircle.normalized;
        }

        protected void SetDirection(Vector2 direction)
        {
            Direction = direction;
        }

        public virtual void Rotate()
        {
            if (_rotateEnabled == false)
                return;

            float newRotation = _targetRotation.eulerAngles.z + (_rotationSpeed * Time.deltaTime);

            _targetRotation = Quaternion.Euler(0f, 0f, newRotation);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation,
                _rotationSpeed * Time.deltaTime);
        }

        public abstract void OnSpawn();

        public abstract void OnDespawn();
    }
}