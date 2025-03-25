using System.Collections.Generic;
using Project.Entities.Enemies;
using UnityEngine;
using VContainer;

namespace Project.Entities
{
    public class EntityRotator : MonoBehaviour
    {
        private EnemyContainer _enemyContainer;

        private List<EnemyBase> RotatableEntities => _enemyContainer.ActiveEnemies;

        [Inject]
        public void Construct( EnemyContainer enemyContainer)
        {
            _enemyContainer = enemyContainer;
        }

        private void Update()
        {
            RotateEntities();
        }

        private void RotateEntities()
        {
            foreach (IRotatable entity in RotatableEntities)
            {
                entity?.Rotate();
            }
        }
    }
}