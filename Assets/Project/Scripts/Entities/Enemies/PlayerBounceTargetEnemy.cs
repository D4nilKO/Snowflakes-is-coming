using Project.Test;
using UnityEngine;

namespace Project.Entities.Enemies
{
    public class PlayerBounceTargetEnemy : StandardEnemy
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out ScreenBounds bounds))
            {
                TurnInPlayerDirection();
            }
        }
    }
}