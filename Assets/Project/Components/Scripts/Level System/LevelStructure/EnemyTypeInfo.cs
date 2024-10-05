using System;

namespace Project.Components.Scripts.Level_System.LevelStructure
{
    [Serializable]
    public struct EnemyTypeInfo
    {
        public string EnemyPrefabName;
        public int MaxSpawnCount;

        public bool IsEquals(EnemyTypeInfo other)
        {
            if (EnemyPrefabName != other.EnemyPrefabName)
            {
                return false;
            }

            if (MaxSpawnCount != other.MaxSpawnCount)
            {
                return false;
            }

            return true;
        }
    }
}