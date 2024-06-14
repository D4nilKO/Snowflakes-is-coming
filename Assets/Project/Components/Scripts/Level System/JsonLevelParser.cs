using System;
using System.IO;
using Project.Components.Scripts.Level_System.LevelStructure;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Components.Scripts.Level_System
{
    public class JsonLevelParser : MonoBehaviour
    {
        [FolderPath] [SerializeField] private string _path;

        [FormerlySerializedAs("_jsonFileName")] [SerializeField]
        private string _fileName;

        [SerializeField] protected TextAsset _levelDataJson;

        protected LevelDataList _levelDataList;

        public virtual LevelDataList GetLevelDataList()
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

            if (TryParseJsonFile(_levelDataJson) == false)
            {
                throw new InvalidOperationException("Failed to load levels from JSON file");
            }

            return _levelDataList;
        }

        protected bool TryGetJsonTextFile(out TextAsset json)
        {
            json = null;

            if (_fileName == string.Empty)
            {
                Debug.LogError("Пустое имя JSON файла");
                return false;
            }
            
            if (_path == string.Empty)
            {
                Debug.LogError("Пустая папка JSON файла");
                return false;
            }
            
            if (!Directory.Exists(_path))
            {
                Debug.LogError($"Папка не найдена: {_path}");
                return false;
            }

            if (!Path.GetExtension(_fileName).Equals(".json", System.StringComparison.OrdinalIgnoreCase))
            {
                _fileName = Path.ChangeExtension(_fileName, ".json");
            }

            string fullPath = Path.Combine(_path, _fileName);

            if (!File.Exists(fullPath))
            {
                Debug.LogError($"JSON файл не найден: {fullPath}");
                return false;
            }

            json = new TextAsset(File.ReadAllText(fullPath));

            if (json == null)
            {
                Debug.LogError($"Указанный JSON файл не найден: {_fileName}");
                return false;
            }

            Debug.Log("JSON файл загружен");

            return true;
        }

        protected bool TryParseJsonFile(TextAsset json)
        {
            _levelDataList = JsonUtility.FromJson<LevelDataList>(json.text);

            if (_levelDataList == null)
            {
                Debug.LogError($"Не удалось загрузить JSON файл: {_fileName}");
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