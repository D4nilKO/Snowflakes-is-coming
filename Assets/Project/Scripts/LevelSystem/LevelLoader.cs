using System;
using System.Linq;
using Project.Data;
using Project.GameState.View;
using Project.LevelSystem.LevelStructure;
using Project.Services.MessageSystem;
using UnityEngine;
using VContainer;

namespace Project.LevelSystem
{
    public class LevelLoader
    {
        private JsonLevelParser _jsonLevelParser;
        private LevelDataInfoView _levelDataInfoView;
        private ProgressData _progressData;
        private IScreenMessageHandler _screenMessageHandler;

        private LevelDataList _levelDataList;

        public event Action<LevelData> LevelSettingsReady;

        [Inject]
        public void Construct(LevelDataInfoView levelDataInfoView, JsonLevelParser jsonLevelParser, ProgressData progressData, IScreenMessageHandler screenMessageHander)
        {
            _levelDataInfoView = levelDataInfoView;
            _jsonLevelParser = jsonLevelParser;
            _progressData = progressData;
            _screenMessageHandler = screenMessageHander;
        }

        public void FetchCurrentLevelSettings()
        {
            _levelDataList = _jsonLevelParser.GetLevelDataList();
            FetchLevelSettings(_levelDataList, _progressData.CurrentLevelNumber);
        }

        private void FetchLevelSettings(LevelDataList levelDataList, int levelNumber)
        {
            if (levelDataList == null)
            {
                Debug.LogError("LevelDataList is null");
                return;
            }

            if (levelDataList.Count == 0)
            {
                Debug.LogError("LevelDataList is empty");
                return;
            }

            int maxLevelsCount = levelDataList.Count;

            if (levelNumber > maxLevelsCount)
            {
                Debug.LogError($"Уровень {levelNumber} не найден в JSON файле, загружен уровень {maxLevelsCount}");
                levelNumber = maxLevelsCount;
            }

            Debug.Log($"Данные уровня №{levelNumber} загружены");

            LevelData currentLevelData = levelDataList.GetLevel(levelNumber - 1);

            if (currentLevelData.EnemyTypesInfo.Any(enemy => enemy.EnemyPrefabName.Contains("C")))
            {
                _screenMessageHandler.ShowMessage(MessageKeys.MessageKeyLevelHasCollectable);
            }
            
            LevelSettingsReady?.Invoke(currentLevelData);

            _levelDataInfoView.SetLevelInfo(currentLevelData);
        }
    }
}