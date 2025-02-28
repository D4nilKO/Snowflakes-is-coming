using Project.NewPhysics;
using UnityEngine;

namespace Project.Entities.Enemies
{
    public class PlayerBounceTargetEnemy : StandardEnemy
    {
        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out ScreenBounds bounds))
            {
                TurnInPlayerDirection();
            }
        }
    }
}