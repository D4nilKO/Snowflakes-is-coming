using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Project.Components.Scripts.Level_System
{
    [Serializable]
    public class LevelDataList
    {
        public LevelDataList([NotNull] IReadOnlyList<LevelData> levels)
        {
            Levels = levels ?? throw new ArgumentNullException(nameof(levels));
        }

        private IReadOnlyList<LevelData> Levels;
        
        public LevelData GetLevel(int index) => Levels[index];
        public int LevelsCount => Levels.Count;

        public LevelDataList Clone()
        {
            return new LevelDataList(Levels);
        }
    }
}