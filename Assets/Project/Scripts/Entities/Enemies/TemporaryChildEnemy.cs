using UnityEngine;
using VContainer;
using static NTC.Pool.NightPool;

namespace Project.Entities.Enemies
{
    public class TemporaryChildEnemy : EnemyBase
    {
        private int _collisionCount = 0;
        private int _maxCollisionCount = 2;

        private EnemyContainer _enemyContainer;

        [Inject]
        public void Construct(EnemyContainer enemyContainer)
        {
            _enemyContainer = enemyContainer;
        }

        protected override void AfterCollisionEnter2D(Collision2D collision)
        {
            base.AfterCollisionEnter2D(collision);

            _collisionCount++;

            if (_collisionCount > _maxCollisionCount)
            {
                _enemyContainer.DespawnEnemy(this);
            }
        }
    }
}