using System;
using System.Collections.Generic;

namespace Project.Components.Scripts.Level_System.LevelStructure
{
    [Serializable]
    public class LevelDataList
    {
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
    }
}