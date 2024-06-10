using UnityEngine;

namespace Project.Components.Scripts.Test
{
    public class TestEnemy : MonoBehaviour
    {
        [SerializeField] private float _speed = 8;
        
        private Rigidbody2D _rigidbody2D;
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            var direction = new Vector2(1, 1).normalized;
            _rigidbody2D.velocity = direction * _speed;
        }
    }
}