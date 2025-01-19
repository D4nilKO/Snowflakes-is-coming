using System.Collections.Generic;
using System.Linq;
using Project.Entities.Character;
using Project.Entities.Enemies;
using UnityEngine;
using VContainer;

namespace Project.Entities
{
    public class EntityMover : MonoBehaviour
    {
        private List<IMovable> _movableEntities = new();
        private List<IRotatable> _rotatableEntities = new();

        private Player _player;
        private EnemyContainer _enemyContainer;
        private IObjectResolver _container;

        [Inject]
        public void Construct(Player player, IObjectResolver container, EnemyContainer enemyContainer)
        {
            _player = player;
            _container = container;
            _enemyContainer = enemyContainer;
        }

        private void Start()
        {
            FindAndAddEntitiesToLists();
        }

        private void FixedUpdate()
        {
            MoveEntities();
        }

        private void Update()
        {
            RotateEntities();
        }

        public void AddEntity(EnemyBase entity)
        {
            _movableEntities.Add(entity);
            _rotatableEntities.Add(entity);
        }

        private void FindAndAddEntitiesToLists()
        {
            var enemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None)
                .Where(enemy => enemy.isActiveAndEnabled).ToArray();

            foreach (var enemy in enemies)
            {
                _container.Inject(enemy);
                AddEntity(enemy);
                _enemyContainer.AddEnemy(enemy);
                enemy.OnSpawn();
            }

            _movableEntities.Add(_player);
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

        private void MoveEntities()
        {
            for (int i = _movableEntities.Count - 1; i >= 0; i--)
            {
                if (_movableEntities[i] as UnityEngine.Object != null)
                {
                    _movableEntities[i].Move();
                }
                else
                {
                    _movableEntities.RemoveAt(i);
                }
            }
        }
    }
}