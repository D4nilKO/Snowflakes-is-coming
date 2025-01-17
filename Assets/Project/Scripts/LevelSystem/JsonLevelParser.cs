using System;
using System.IO;
using Project.Data;
using Project.LevelSystem.LevelStructure;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace Project.LevelSystem
{
    public class JsonLevelParser : MonoBehaviour
    {
        [SerializeField]
        private bool _usePath;

        [SerializeField, FilePath, ShowIf(nameof(_usePath))]
        private string _path;

        [SerializeField, HideIf(nameof(_usePath))]
        protected TextAsset _levelDataJson;

        protected LevelDataList LevelDataList;

        private ProgressData _progressData;

        [Inject]
        public void Construct(ProgressData progressData)
        {
            _progressData = progressData;
        }

        public virtual LevelDataList GetLevelDataList()
        {
            if (LevelDataList != null)
                return LevelDataList;

            if (_usePath)
                LoadJsonFromFile();

            if (TryParseJsonFile(_levelDataJson) == false)
                throw new InvalidOperationException("Failed to load levels from JSON file");

            _progressData.SetMaxLevelsCount(LevelDataList.Count);

            return LevelDataList;
        }

        private void LoadJsonFromFile()
        {
            if (string.IsNullOrEmpty(_path))
            {
                Debug.LogError("Invalid file name or path");
                return;
            }

            if (File.Exists(_path) == false)
            {
                Debug.LogError($"Directory not found: {_path}");
                return;
            }

            _levelDataJson = new TextAsset(File.ReadAllText(_path));
        }

        protected bool TryParseJsonFile(TextAsset json)
        {
            LevelDataList = JsonUtility.FromJson<LevelDataList>(json.text);

            if (LevelDataList == null)
            {
                Debug.LogError($"Failed to parse JSON file {json.name}");
                return false;
            }

            int maxLevelsCount = LevelDataList.Count;

            if (maxLevelsCount <= 0)
            {
                Debug.LogError("Levels not loaded, check JSON file and class names.");
                return false;
            }

            return true;
        }
    }
}