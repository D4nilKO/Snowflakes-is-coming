using NTC.Global.Pool;
using Project.Entities.Character;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Entities.Enemies
{
    public abstract class EnemyBase : Entity, IMovable, IPoolItem
    {
        [SerializeField, Range(0f, 20f)]
        private float _speed = 5f;

        [SerializeField, Range(1f, 359f)]
        private float _rotationSpeed = 10f;

        [SerializeField]
        private bool _rotateEnabled = true;

        private Quaternion _targetRotation;

        private Vector2 _direction;
        protected float Speed => _speed;

        protected Vector2 Direction
        {
            get => _direction;
            set
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
            Direction = Random.insideUnitCircle;
        }

        protected void TurnInPlayerDirection()
        {
            Vector3 playerPosition = Player.Instance.transform.position;
            Direction = playerPosition - transform.position;
        }

        protected Vector2 GetNewPosition(Vector2 currentPosition)
        {
            return currentPosition + (Direction * (Speed * Time.fixedDeltaTime));
        }

        protected void TryReflect(ref Vector2 newPosition)
        {
            CheckOutOfBounds(newPosition, out bool isOutOfBoundsX, out bool isOutOfBoundsY);
            ReflectDirection(isOutOfBoundsX, isOutOfBoundsY);
        }

        private void CheckOutOfBounds(Vector2 newPosition, out bool isOutOfBoundsX, out bool isOutOfBoundsY)
        {
            float halfScreenWidth = ScreenWidth * 0.5f;
            float halfScreenHeight = ScreenHeight * 0.5f;

            float halfObjectWidth = ObjectWidth * 0.5f;
            float halfObjectHeight = ObjectHeight * 0.5f;

            isOutOfBoundsX = newPosition.x < -halfScreenWidth + halfObjectWidth ||
                newPosition.x > halfScreenWidth - halfObjectWidth;

            isOutOfBoundsY = newPosition.y < -halfScreenHeight + halfObjectHeight ||
                newPosition.y > halfScreenHeight - halfObjectHeight;
        }

        protected virtual void ReflectDirection(bool isOutOfBoundsX, bool isOutOfBoundsY)
        {
            if (isOutOfBoundsX)
                ReflectHorizontal();

            if (isOutOfBoundsY)
                ReflectVertical();
        }

        private void ReflectHorizontal()
        {
            Direction = new Vector2(-Direction.x, Direction.y);
        }

        private void ReflectVertical()
        {
            Direction = new Vector2(Direction.x, -Direction.y);
        }

        protected void ClampPosition(ref Vector2 position)
        {
            Bounds bounds = ObjectCollider.bounds;

            position.x = Mathf.Clamp(
                position.x,
                (-ScreenWidth / 2f) + bounds.extents.x,
                (ScreenWidth / 2f) - bounds.extents.x);

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