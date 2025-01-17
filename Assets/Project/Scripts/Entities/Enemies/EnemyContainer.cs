using System.Collections.Generic;
using System.Linq;
using NTC.Global.Pool;
using Sirenix.Utilities;
using UnityEngine;

namespace Project.Entities.Enemies
{
    public class EnemyContainer : MonoBehaviour
    {
        private float _slowFactor;

        private HashSet<EnemyBase> _activeEnemies = new();

        public void SetSlowFactor(float slowFactor)
        {
            _slowFactor = slowFactor;
        }

        public void AddEnemy(EnemyBase enemy)
        {
            _activeEnemies.Add(enemy);
            enemy.SlowFactor = _slowFactor;
        }

        public void ClearActiveEnemies()
        {
            _activeEnemies.AddRange(GetComponentsInChildren<EnemyBase>());

            foreach (EnemyBase enemy in _activeEnemies)
            {
                NightPool.Despawn(enemy);
            }
        }

        public List<EnemyBase> GetEnemies()
        {
            return _activeEnemies.ToList();
        }
    }
}