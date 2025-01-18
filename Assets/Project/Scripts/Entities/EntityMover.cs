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

        private IObjectResolver _container;

        [Inject]
        public void Construct(Player player, IObjectResolver container)
        {
            _player = player;
            _container = container;
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

            _rotatableEntities.AddRange(enemies);

            _movableEntities.AddRange(enemies);
            _movableEntities.Add(_player);

            foreach (var enemy in enemies)
            {
                _container.Inject(enemy);
            }
        }

        private void RotateEntities()
        {
            for (int i = _rotatableEntities.Count - 1; i >= 0; i--)
            {
                try
                {
                    _rotatableEntities[i].Rotate();
                }
                catch
                {
                    _rotatableEntities.RemoveAt(i);
                }
            }
        }

        private void MoveEntities()
        {
            for (int i = _movableEntities.Count - 1; i >= 0; i--)
            {
                try
                {
                    _movableEntities[i].Move();
                }
                catch
                {
                    _movableEntities.RemoveAt(i);
                }
            }
        }
    }
}