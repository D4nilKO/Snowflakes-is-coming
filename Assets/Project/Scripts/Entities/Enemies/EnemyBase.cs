using NTC.Global.Pool;
using Project.Entities.Character;
using Project.NewPhysics;
using Project.VisualEffects;
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

        private Vector2 _direction;

        public SnowflakeEffectSpawner SnowflakeEffectSpawner;

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

        protected Vector2 Direction
        {
            get => Rigidbody2D.velocity.normalized;
            set => Rigidbody2D.velocity = value.normalized * Speed;
        }

        private void UpdateRigidbodySpeed()
        {
            Rigidbody2D.velocity = Direction * Speed;
        }

        protected virtual void OnEnable()
        {
            UpdateRigidbodySpeed();
        }

        public virtual void OnSpawn()
        {
            SetDirectionOnSpawn();
        }

        public virtual void OnDespawn()
        {
            SlowFactor = 0f;
            SnowflakeEffectSpawner.DespawnEffect(transform.position);
        }

        public void Rotate()
        {
            if (_rotateEnabled == false || gameObject.activeInHierarchy == false)
                return;

            transform.Rotate(0f, 0f, _rotationSpeed * Time.deltaTime);
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
            Gizmos.DrawRay(transform.position, transform.position + (Vector3)Direction * 100f);
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out ScreenBounds bounds))
            {
                AfterCollisionEnter2D(collision);
                
                SnowflakeEffectSpawner.SpawnBounceEffect(collision);
            }
        }

        protected virtual void AfterCollisionEnter2D(Collision2D collision)
        {
            Invoke(nameof(ApplyBounceCorrection), 0f);
        }

        private void ApplyBounceCorrection()
        {
            float tolerance = 10f;
            float minDeviation = 15f; // Минимальное отклонение от 90 градусов

            float alpha = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
            float correctedAlpha = alpha;

            float remainder = Mathf.Abs(alpha % 90);

            // Проверяем, если угол слишком близок к 0, 90, 180, 270
            if (remainder < tolerance || remainder > 90 - tolerance)
            {
                if (remainder < tolerance)
                {
                    correctedAlpha += (alpha > 0 ? minDeviation : -minDeviation);
                }
                else
                {
                    correctedAlpha -= (alpha > 0 ? minDeviation : -minDeviation);
                }

                // Дополнительная проверка, не ведет ли новый угол снова в стену
                float newRemainder = Mathf.Abs(correctedAlpha % 90);

                if (newRemainder < tolerance || newRemainder > 90 - tolerance)
                {
                    correctedAlpha += (Random.value > 0.5f ? 1 : -1) * 2 * minDeviation;
                }
            }

            Direction = new Vector2(Mathf.Cos(correctedAlpha * Mathf.Deg2Rad),
                Mathf.Sin(correctedAlpha * Mathf.Deg2Rad));
        }
    }
}