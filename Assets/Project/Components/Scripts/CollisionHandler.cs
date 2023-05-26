using System;
using UnityEngine;

namespace Project.Components.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class CollisionHandler : MonoBehaviour
    {
        [SerializeField] private ButtonHandler buttonHandler;

        private void OnTriggerEnter2D(Collider2D col)
        {
            EnemyBase enemyBase;
            
            if (col.gameObject.TryGetComponent(out enemyBase))
            {
                buttonHandler.LostGame();
            }
        }
    }
}