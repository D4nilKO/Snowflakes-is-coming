using UnityEngine;

namespace Project.Components.Scripts.Entities.Enemies
{
    public class StandardEnemy : EnemyBase
    {
        public override void Move()
        {
            Vector2 currentPosition = Rigidbody2D.position;
            Vector2 newPosition = currentPosition + (Direction * (Speed * Time.fixedDeltaTime));

            CheckOutOfBounds(ref newPosition);

            Rigidbody2D.MovePosition(newPosition);
        }

        public override void OnSpawn()
        {
            SetRandomDirection();
        }

        public override void OnDespawn()
        {
        }
    }
}