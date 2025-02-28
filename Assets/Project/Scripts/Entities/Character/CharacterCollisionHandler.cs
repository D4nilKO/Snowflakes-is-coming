using System;
using Project.Entities.Enemies;
using UnityEngine;

namespace Project.Entities.Character
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class CharacterCollisionHandler : MonoBehaviour
    {
        [SerializeField]
        private bool _isInvincible;

        public event Action DamageTaken;

        public void ActivateInvincibility(float duration)
        {
            if (duration <= 0)
                return;

            if (_isInvincible)
                return;

            ActivateInvincibility();
            Invoke(nameof(DeactivateInvincibility), duration);
        }

        public void ActivateInvincibility() => _isInvincible = true;

        public void DeactivateInvincibility() => _isInvincible = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            CheckCollision(collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            CheckCollision(collision);
        }

        private void CheckCollision(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out ICollectable collectable))
            {
                collectable.Collect();
            }

            if (_isInvincible)
                return;

            if (collision.gameObject.TryGetComponent(out IDamageDealer damageDealer))
            {
                damageDealer.DealDamage();
                DamageTaken?.Invoke();
            }
        }
    }
}