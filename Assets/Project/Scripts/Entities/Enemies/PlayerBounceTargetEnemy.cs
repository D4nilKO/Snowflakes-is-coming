using UnityEngine;

namespace Project.Entities.Enemies
{
    public class PlayerBounceTargetEnemy : StandardEnemy
    {
        protected override void AfterCollisionEnter2D(Collision2D collision)
        {
            TurnInPlayerDirection();
        }
    }
}