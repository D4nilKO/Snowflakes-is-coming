using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Components.Scripts
{
    [DisallowMultipleComponent]
    public class EnemyManager : MonoBehaviour
    {
        static public List<EnemyBase> enemies = new List<EnemyBase>();

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
                enemy.Move();
            }
        }
    }
}