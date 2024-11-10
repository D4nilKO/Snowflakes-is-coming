using UnityEngine;
using static NTC.Global.Pool.NightPool;

namespace Project.Entities.Enemies
{
    public class EnemyContainer : MonoBehaviour
    {
        public void ClearActiveEnemies()
        {
            EnemyBase[] enemies = GetComponentsInChildren<EnemyBase>();

            foreach (EnemyBase enemy in enemies)
            {
                Despawn(enemy);
            }
        }
    }
}