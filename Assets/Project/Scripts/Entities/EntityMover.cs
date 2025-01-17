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

        [Inject]
        public void Construct(Player player)
        {
            _player = player;
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

        public void AddMovableEntity(IMovable movableEntity)
        {
            _movableEntities.Add(movableEntity);
        }

        private void FindAndAddEntitiesToLists()
        {
            _rotatableEntities.AddRange(FindObjectsByType<EnemyBase>(FindObjectsSortMode.None)
                .Where(enemy => enemy.isActiveAndEnabled));

            _movableEntities.AddRange(FindObjectsByType<EnemyBase>(FindObjectsSortMode.None)
                .Where(enemy => enemy.isActiveAndEnabled));

            _movableEntities.Add(_player);
        }

        private void RotateEntities()
        {
            foreach (IRotatable rotatableEntity in _rotatableEntities.Where(entity => entity != null))
            {
                rotatableEntity.Rotate();
            }
        }

        private void MoveEntities()
        {
            foreach (IMovable movableEntity in _movableEntities.Where(entity => entity != null))
            {
                movableEntity.Move();
            }
        }
    }
}