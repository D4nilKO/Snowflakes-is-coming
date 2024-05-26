using System.Linq;
using UnityEngine;
using static NTC.Global.Pool.NightPool;

namespace Project.Components.Scripts.Entities.Enemies
{
    public class EnemyContainer : MonoBehaviour
    {
        public void ClearActiveEnemies()
        {
            EnemyBase[] enemies = GetComponentsInChildren<EnemyBase>();
            
            foreach (EnemyBase enemy in enemies.Where(enemy => enemy.isActiveAndEnabled))
            {
                Despawn(enemy);
            }
        }
    }
}