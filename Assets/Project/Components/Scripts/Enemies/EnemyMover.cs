using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Components.Scripts
{
    [DisallowMultipleComponent]
    public class EnemyMover : MonoBehaviour
    {
        [HideInInspector] public List<EnemyBase> enemies;
        private Character character;

        private void Start()
        {
            enemies = FindObjectsOfType<EnemyBase>().ToList();
            character = FindObjectOfType<Character>();
        }

        private void FixedUpdate()
        {
            foreach (var enemy in enemies.Where(enemy => enemy.isActiveAndEnabled))
            {
                enemy.Move();
                enemy.Rotate();
            }

            character.Move();
        }
    }
}