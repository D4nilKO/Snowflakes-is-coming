using UnityEngine;

namespace Project.Components.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class CharacterCollisionHandler : MonoBehaviour
    {
        private GameStateMachine gameStateMachine;

        private void Awake()
        {
            gameStateMachine = FindObjectOfType<GameStateMachine>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.TryGetComponent(out EnemyBase _))
            {
                gameStateMachine.LostGame();
            }
        }
    }
}