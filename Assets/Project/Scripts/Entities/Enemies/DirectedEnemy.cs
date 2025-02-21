using UnityEngine;

namespace Project.Entities.Enemies
{
    public class DirectedEnemy: EnemyBase
    {
        [SerializeField]
        private Vector2 _startDirection;

        protected override void SetDirectionOnSpawn()
        {
            Direction = _startDirection;
        }
    }
}