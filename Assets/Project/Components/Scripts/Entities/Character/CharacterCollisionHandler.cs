using Project.Entities.Enemies;
using Project.GameState;
using UnityEngine;

namespace Project.Entities.Character
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class CharacterCollisionHandler : MonoBehaviour
    {
        [SerializeField] private GameOutcome _gameOutcome;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.TryGetComponent(out EnemyBase _))
            {
                _gameOutcome.LostGame();
            }
        }
    }
}