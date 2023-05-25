using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Components.Scripts
{
    [DisallowMultipleComponent]
    public class EnemyMover : MonoBehaviour
    {
        public List<EnemyBase> enemies;

        private void Start()
        {
            enemies = FindObjectsOfType<EnemyBase>().ToList();
        }

        private void FixedUpdate()
        {
            foreach (var enemy in enemies.Where(enemy => enemy.isActiveAndEnabled))
            {
                enemy.Move();
                enemy.Rotate();
            }
        }
    }
}