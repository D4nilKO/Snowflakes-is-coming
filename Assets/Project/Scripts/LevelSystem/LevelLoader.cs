using System;
using Project.Data;
using Project.GameState.View;
using Project.LevelSystem.LevelStructure;
using Project.Timing;
using UnityEngine;
using VContainer;

namespace Project.LevelSystem
{
    public class LevelLoader
    {
        private JsonLevelParser _jsonLevelParser;

        private PauseHandler _pauseHandler;
        private LevelInfoView _levelInfoView;

        private LevelDataList _levelDataList;

        public event Action<LevelData> LevelSettingsReady;

        [Inject]
        public void Construct(PauseHandler pauseHandler, LevelInfoView levelInfoView, JsonLevelParser jsonLevelParser)
        {
            _pauseHandler = pauseHandler;
            _levelInfoView = levelInfoView;
            _jsonLevelParser = jsonLevelParser;
        }

        public void FetchCurrentLevelSettings()
        {
            _levelDataList = _jsonLevelParser.GetLevelDataList();
            FetchLevelSettings(_levelDataList, ProgressData.CurrentLevelNumber);
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
                Debug.LogError($"Уровень {levelNumber} не найден в JSON файле");
                return;
            }

            Debug.Log($"Данные уровня №{levelNumber} загружены");

            LevelData currentLevelData = levelDataList.GetLevel(levelNumber - 1);
            LevelSettingsReady?.Invoke(currentLevelData);

            _levelInfoView.SetLevelInfo(currentLevelData);

            _pauseHandler.ForceInGamePause();
        }
    }
}