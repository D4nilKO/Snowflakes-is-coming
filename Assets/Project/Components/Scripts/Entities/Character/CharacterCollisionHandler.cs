using Project.Components.Scripts.Entities.Enemies;
using Project.Components.Scripts.GameState;
using UnityEngine;

namespace Project.Components.Scripts.Entities.Character
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