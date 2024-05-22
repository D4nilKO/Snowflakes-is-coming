using Project.Components.Scripts.Entities.Enemies;
using UnityEngine;

namespace Project.Components.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class CharacterCollisionHandler : MonoBehaviour
    {
        [SerializeField] private GameStateMachine _gameStateMachine;

        public void Awake()
        {
            _gameStateMachine = FindObjectOfType<GameStateMachine>();// убрать 
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.TryGetComponent(out EnemyBase _))
            {
                _gameStateMachine.LostGame();
            }
        }
    }
}