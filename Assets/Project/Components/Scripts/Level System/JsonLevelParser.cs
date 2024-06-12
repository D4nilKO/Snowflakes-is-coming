using System;
using System.IO;
using Project.Components.Scripts.Level_System.LevelStructure;
using UnityEngine;

namespace Project.Components.Scripts.Level_System
{
    public class JsonLevelParser : MonoBehaviour
    {
        [FolderPath] [SerializeField] private string _path;
        [SerializeField] private string _jsonFileName;

        [SerializeField] private TextAsset _levelDataJson;

        private LevelDataList _levelDataList;

        public LevelDataList GetLevelDataList()
        {
            if (_levelDataList != null)
            {
                return _levelDataList;
            }

            if (_levelDataJson == null)
            {
                if (TryGetJsonTextFile(out _levelDataJson) == false)
                {
                    throw new InvalidOperationException("Failed to load levels from resources");
                }
            }

            if (!TryParseJsonFile(_levelDataJson))
            {
                throw new InvalidOperationException("Failed to load levels from JSON file");
            }

            return _levelDataList;
        }

        private bool TryGetJsonTextFile(out TextAsset json)
        {
            json = null;

            if (_jsonFileName == string.Empty)
            {
                Debug.LogError("Пустое имя JSON файла");
                return false;
            }

            string fullPath = Path.Combine(_path, _jsonFileName);

            if (!File.Exists(fullPath))
            {
                Debug.LogError($"JSON файл не найден: {fullPath}");
                return false;
            }

            json = Resources.Load<TextAsset>(_jsonFileName);

            if (json == null)
            {
                Debug.LogError($"Указанный JSON файл не найден: {_jsonFileName}");
                return false;
            }

            return true;
        }

        private bool TryParseJsonFile(TextAsset json)
        {
            _levelDataList = JsonUtility.FromJson<LevelDataList>(json.text);

            if (_levelDataList == null)
            {
                Debug.LogError($"Не удалось загрузить JSON файл: {_jsonFileName}");
                return false;
            }

            int maxLevelsCount = _levelDataList.LevelsCount;

            if (maxLevelsCount <= 0)
            {
                Debug.LogError("Уровни не загрузились, проверьте названия переменных внутри JSON и pure класов.");
                return false;
            }

            return true;
        }
    }
}