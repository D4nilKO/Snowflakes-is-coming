using System;

namespace Project.LevelSystem.LevelStructure
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