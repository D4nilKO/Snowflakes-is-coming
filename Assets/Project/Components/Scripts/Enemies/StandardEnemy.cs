using UnityEngine;

namespace Project.Components.Scripts.Enemies
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
            Vector2 currentPosition = Rb2D.position;
            Vector2 newPosition = currentPosition + Direction * (speed * Time.fixedDeltaTime);
            
            CheckOutOfBounds(ref newPosition);
            
            Rb2D.MovePosition(newPosition);
        }
    }
}