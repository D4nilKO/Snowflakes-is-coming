using System;
using UnityEngine;

namespace Project.Components.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class CollisionHandler : MonoBehaviour
    {
        private GameStateMachine gameStateMachine;

        private void Awake()
        {
            gameStateMachine = FindObjectOfType<GameStateMachine>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            EnemyBase enemyBase;

            if (col.gameObject.TryGetComponent(out enemyBase))
            {
                gameStateMachine.LostGame();
            }
        }
    }
}