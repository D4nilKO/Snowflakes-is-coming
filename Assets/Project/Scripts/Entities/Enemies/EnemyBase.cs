using NTC.Global.Pool;
using Project.Entities.Character;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Entities.Enemies
{
    public abstract class EnemyBase : Entity, IPoolItem, IRotatable
    {
        [SerializeField, Range(0f, 20f)]
        private float _speed = 5f;

        [SerializeField, Range(1f, 359f)]
        private float _rotationSpeed = 10f;

        [SerializeField]
        private bool _rotateEnabled = true;

        private float _slowFactor = 0f;

        private Quaternion _targetRotation;
        private Vector2 _direction;

        [ShowInInspector]
        public float SlowFactor
        {
            get => _slowFactor;
            set
            {
                _slowFactor = Mathf.Clamp01(value);
                UpdateRigidbodySpeed();
            }
        }

        protected float Speed
        {
            get => _speed * (1f - _slowFactor);
            set
            {
                UpdateRigidbodySpeed();
                _speed = value;
            }
        }

        private Vector2 Direction
        {
            get => Rigidbody2D.velocity.normalized;
            set => Rigidbody2D.velocity = value.normalized * Speed;
        }

        private void UpdateRigidbodySpeed()
        {
            Rigidbody2D.velocity = Direction * Speed;
        }

        protected virtual void Start()
        {
            _targetRotation = transform.rotation;
        }

        private void OnEnable()
        {
            UpdateRigidbodySpeed();
        }

        public virtual void OnSpawn()
        {
            SetDirectionOnSpawn();
            OnEnable();
        }

        public virtual void OnDespawn()
        {
            SlowFactor = 0f;
        }

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

        protected void TurnInPlayerDirection()
        {
            Vector3 playerPosition = Player.Instance.transform.position;
            Direction = playerPosition - transform.position;
        }

        protected virtual void SetDirectionOnSpawn()
        {
            SetRandomDirection();
        }

        private void SetRandomDirection()
        {
            Direction = Random.insideUnitCircle;
        }

        private void OnDrawGizmos()
        {
            if (Rigidbody2D == null)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.position + (Vector3)Direction * 1000f);
        }
    }
}