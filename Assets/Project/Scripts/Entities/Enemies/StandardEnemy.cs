using UnityEngine;

namespace Project.Entities.Enemies
{
    public class StandardEnemy : EnemyBase
    {
        public override void Move()
        {
            Vector2 newPosition = GetNewPosition(Rigidbody2D.position);

            TryReflect(ref newPosition);
            ClampPosition(ref newPosition);

            Rigidbody2D.MovePosition(newPosition);
        }

        public override void OnSpawn()
        {
            SetRandomDirection();
        }

        public override void OnDespawn() { }
    }
}