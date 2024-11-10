using System;
using System.Collections.Generic;

namespace Project.LevelSystem.LevelStructure
{
    [Serializable]
    public class LevelDataList
    {
        public LevelDataList()
        {
            Levels = new List<LevelData>();
        }

        public List<LevelData> Levels;

        public LevelData GetLevel(int index) => Levels[index];

        public int LevelsCount
        {
            get
            {
                if (Levels == null)
                {
                    throw new InvalidOperationException();
                }

                return Levels.Count;
            }
        }

        public bool CheckLevelUniqueness(LevelData newLevel)
        {
            foreach (LevelData oldLevel in Levels)
            {
                if (newLevel.IsEqual(oldLevel) == false)
                {
                    continue;
                }

                return false;
            }

            return true;
        }
    }
}