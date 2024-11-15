﻿using System.Collections.Generic;
using System.Linq;
using Project.Entities.Character;
using Project.Entities.Enemies;
using UnityEngine;

namespace Project.Entities
{
    [DisallowMultipleComponent]
    public class EntityMover : MonoBehaviour
    {
        [SerializeField]
        private Player _player;

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
            _movableEntities.AddRange(FindObjectsByType<EnemyBase>(FindObjectsSortMode.None)
                .Where(enemy => enemy.isActiveAndEnabled));
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