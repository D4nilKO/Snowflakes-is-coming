using Project.Entities.Enemies;
using Project.GameState;
using UnityEngine;
using VContainer;

namespace Project.Entities.Character
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class CharacterCollisionHandler : MonoBehaviour
    {
        [SerializeField]
        private bool _isInvincible;

        private GameOutcome _gameOutcome;

        [Inject]
        public void Construct(GameOutcome gameOutcome)
        {
            _gameOutcome = gameOutcome;
            _isInvincible = false;
        }

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
            if (!_isInvincible && collision.gameObject.TryGetComponent(out EnemyBase enemy))
            {
                _gameOutcome.LostGame();
            }
        }
    }
}