using UnityEngine;

namespace Project.Components.Scripts.Entities.Enemies
{
    public class StandardEnemy : EnemyBase
    {
        protected override void Start()
        {
            base.Start();
            
            Direction = GetRandomDirection();
        }

        public override void Move()
        {
            Vector2 currentPosition = Rigidbody2D.position;
            Vector2 newPosition = currentPosition + Direction * (_speed * Time.fixedDeltaTime);
            
            CheckOutOfBounds(ref newPosition);
            
            Rigidbody2D.MovePosition(newPosition);
        }
    }
}