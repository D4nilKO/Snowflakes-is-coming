﻿using System;
using Project.Components.Scripts.Level_System.LevelStructure;

namespace Project.Components.Scripts.Level_System.LevelCreatingSystem
{
    public class RuntimeJsonParser : JsonLevelParser
    {
        public override LevelDataList GetLevelDataList()
        {
            _levelDataList = null;

            if (_levelDataJson == null)
            {
                if (TryGetJsonTextFile(out _levelDataJson) == false)
                {
                    throw new InvalidOperationException("Failed to load levels");
                }
            }

            if (TryParseJsonFile(_levelDataJson) == false)
            {
                throw new InvalidOperationException("Failed to load levels from JSON file");
            }

            return _levelDataList;
        }
    }
}