using System.Collections.Generic;
using System.Linq;
using Project.Entities.Enemies;
using UnityEngine;
using VContainer;

namespace Project.Entities
{
    public class EntityRotator : MonoBehaviour
    {
        private List<IRotatable> _rotatableEntities = new();

        private EnemyContainer _enemyContainer;
        private IObjectResolver _container;

        [Inject]
        public void Construct(IObjectResolver container, EnemyContainer enemyContainer)
        {
            _container = container;
            _enemyContainer = enemyContainer;
        }

        private void Start()
        {
            FindAndAddEntitiesToList();
        }

        private void Update()
        {
            RotateEntities();
        }

        public void AddEntity(EnemyBase entity)
        {
            _rotatableEntities.Add(entity);
        }

        private void FindAndAddEntitiesToList()
        {
            var enemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None)
                .Where(enemy => enemy.isActiveAndEnabled);

            foreach (var enemy in enemies)
            {
                _container.Inject(enemy);
                AddEntity(enemy);
                _enemyContainer.AddEnemy(enemy);
                enemy.OnSpawn();
            }
        }

        private void RotateEntities()
        {
            for (int i = _rotatableEntities.Count - 1; i >= 0; i--)
            {
                if (_rotatableEntities[i] as UnityEngine.Object != null)
                {
                    _rotatableEntities[i].Rotate();
                }
                else
                {
                    _rotatableEntities.RemoveAt(i);
                }
            }
        }
    }
}