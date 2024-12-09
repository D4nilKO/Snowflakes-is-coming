﻿using Project.Entities.Enemies;
using Project.GameState;
using UnityEngine;

namespace Project.Entities.Character
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class CharacterCollisionHandler : MonoBehaviour
    {
        [SerializeField]
        private GameOutcome _gameOutcome;

        [SerializeField]
        private bool _isInvincible;

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

        private void Awake()
        {
            if (_gameOutcome == null)
            {
                _gameOutcome = GetComponent<GameOutcome>();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_isInvincible && collision.gameObject.TryGetComponent(out EnemyBase enemy))
            {
                _gameOutcome.LostGame();
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!_isInvincible && collision.gameObject.TryGetComponent(out EnemyBase enemy))
            {
                _gameOutcome.LostGame();
            }
        }
    }
}