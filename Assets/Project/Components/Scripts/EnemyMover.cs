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
            EnemyBase.TakeCameraSize(Camera.main);
        }

        private void FixedUpdate()
        {
            for (var index = 0; index < enemies.Count; index++)
            {
                var enemy = enemies[index];
                if (enemy.isActiveAndEnabled)
                {
                    enemy.Move();
                }
            }
        }
    }
}