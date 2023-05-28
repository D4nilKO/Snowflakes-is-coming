using System;
using UnityEngine;

namespace Project.Components.Scripts
{
    [Serializable]
    public class EnemyTypeInfo
    {
        public GameObject enemyPrefab;
        public int maxSpawnCount;
    }
}