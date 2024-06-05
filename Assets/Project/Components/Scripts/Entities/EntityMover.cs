using System.Collections.Generic;
using System.Linq;
using Project.Components.Scripts.Entities.Character;
using Project.Components.Scripts.Entities.Enemies;
using UnityEngine;

namespace Project.Components.Scripts.Entities
{
    [DisallowMultipleComponent]
    public class EntityMover : MonoBehaviour
    {
        [SerializeField] private Player _player;

        private List<IMovable> _movableEntities = new();

        public void AddMovableEntity(IMovable movableEntity)
        {
            _movableEntities.Add(movableEntity);
        }

        private void Start()
        {
            FindAndAddMovableEntities();
        }

        private void FixedUpdate()
        {
            MoveEntities();
        }

        private void Update()
        {
            RotateEntities();
        }

        private void FindAndAddMovableEntities()
        {
            _movableEntities.AddRange(FindObjectsOfType<EnemyBase>().Where(enemy => enemy.isActiveAndEnabled));
        }

        private void RotateEntities()
        {
            foreach (EnemyBase enemy in _movableEntities.OfType<EnemyBase>())
            {
                enemy.Rotate();
            }
        }

        private void MoveEntities()
        {
            foreach (IMovable movableEntity in _movableEntities.Where(entity => entity != null))
            {
                movableEntity.Move();
            }

            if (_player != null)
            {
                _player.Move();
            }
        }
    }
}