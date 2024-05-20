using System;
using UnityEngine.Serialization;

namespace Project.Components.Scripts.Entities.Enemies
{
    [Serializable]
    public class EnemyTypeInfo
    {
        [FormerlySerializedAs("enemyPrefabName")] public string EnemyPrefabName;
        [FormerlySerializedAs("maxSpawnCount")] public int MaxSpawnCount;
    }
}