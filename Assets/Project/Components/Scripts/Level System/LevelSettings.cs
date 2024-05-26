using System;
using Project.Components.Scripts.Data;
using UnityEngine;

namespace Project.Components.Scripts.Level_System
{
    [DisallowMultipleComponent]
    public class LevelSettings : MonoBehaviour
    {
        [SerializeField] private string _jsonFileName;

        [SerializeField] [Header("Для ознакомления, загружается в начале игры")]
        private LevelData _currentLevelData;

        public event Action<LevelData> LevelSettingsReady;

        private void Start()
        {
            SetLevelSettings();
            Debug.Log("level settings start");
        }

        public void SetLevelSettings()
        {
            TextAsset json = Resources.Load<TextAsset>(_jsonFileName);

            if (json == null)
            {
                Debug.LogError($"Указанный JSON файл не найден: {_jsonFileName}");
                return;
            }

            LevelDataList levelDataList = JsonUtility.FromJson<LevelDataList>(json.text);
            int maxLevelsCount = levelDataList.Levels.Count;

            if (maxLevelsCount == 0)
            {
                Debug.LogError("Уровень не загрузился, проверьте нейминг переменных внутри JSON и pure класов.");
                return;
            }

            ProgressData.MaxLevelsCount = maxLevelsCount;
            int currentLevelNumber = ProgressData.CurrentLevelNumber;

            if (currentLevelNumber > maxLevelsCount)
            {
                Debug.LogError($"Уровень {currentLevelNumber} не найден в JSON файле: {_jsonFileName}");
                return;
            }

            Debug.Log("Данные уровня загружены: " + currentLevelNumber);

            LevelData levelData = levelDataList.Levels[currentLevelNumber - 1];
            LevelSettingsReady?.Invoke(levelData);

            _currentLevelData = levelData;
        }
    }
}