using System.Collections.Generic;
using System.Linq;
using Project.Components.Scripts.Entities.Character;
using Project.Components.Scripts.Entities.Enemies;
using UnityEngine;

namespace Project.Components.Scripts
{
    [DisallowMultipleComponent]
    public class EntityMover : MonoBehaviour
    {
        [SerializeField] private Character _character;

        [SerializeField] private List<EnemyBase> _enemies;

        private void Start()
        {
            _enemies = FindObjectsOfType<EnemyBase>().ToList();
        }
        
        private void Update()
        {
            foreach (EnemyBase enemy in _enemies.Where(enemy => enemy.isActiveAndEnabled))
            {
                enemy.Rotate();
            }
        }

        private void FixedUpdate()
        {
            foreach (EnemyBase enemy in _enemies.Where(enemy => enemy.isActiveAndEnabled))
            {
                enemy.Move();
            }

            _character.Move();
        }

        public void AddEnemy(EnemyBase enemy)
        {
            _enemies.Add(enemy);
        }
    }
}