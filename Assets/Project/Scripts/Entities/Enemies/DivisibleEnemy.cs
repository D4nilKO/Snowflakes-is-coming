using UnityEngine;
using VContainer;
using static NTC.Pool.NightPool;

namespace Project.Entities.Enemies
{
    // Enemy 8 - при отскоках делится на 2 таких же, но только один из них продолжит делиться,
    // а второй при столкновении со стеной исчезнет.
    public class DivisibleEnemy : EnemyBase
    {
        private EnemySpawner _enemySpawner;

        [Inject]
        public void Construct(EnemySpawner enemySpawner)
        {
            _enemySpawner = enemySpawner;
        }

        [SerializeField]
        private EnemyBase _childPrefab;

        protected override void AfterCollisionEnter2D(Collision2D collision)
        {
            base.AfterCollisionEnter2D(collision);

            Split();
        }

        private void Split()
        {
            _enemySpawner.SpawnEnemy(_childPrefab, transform.position);
        }
    }
}