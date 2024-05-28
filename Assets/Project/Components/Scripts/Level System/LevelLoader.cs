using Project.Components.Scripts.Data;
using Project.Components.Scripts.Entities.Character;
using Project.Components.Scripts.Entities.Enemies;
using Project.Components.Scripts.GameState;
using Project.Components.Scripts.Timing;
using UnityEngine;

namespace Project.Components.Scripts.Level_System
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private GameOutcome _gameOutcome;
        [SerializeField] private Character _player;
        [SerializeField] private JsonLevelParser _jsonLevelParser;
        [SerializeField] private TimersView _timersView;
        [SerializeField] private EnemyContainer _enemyContainer;
        [SerializeField] private LevelTextView _levelTextView;
        [SerializeField] private ProgressData _progressData;
        
        private LevelData _levelData;

        private void Awake()
        {
            SubscribeEvents();
            Debug.Log("level loader awake");
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _jsonLevelParser.LevelSettingsReady += SetLevelData;
        }

        private void UnsubscribeEvents()
        {
            _jsonLevelParser.LevelSettingsReady -= SetLevelData;
        }

        private void SetLevelData(LevelData levelData)
        {
            _levelData = levelData;
            
            InitAll();
        }

        private void InitAll()
        {
            _enemySpawner.Init(_levelData.EnemyTypesInfo, _levelData.TimeToSpawn);
            _gameOutcome.Init(_levelData.TimeToSurvive);
            _timersView.Init(_levelData.TimeToSurvive);
            _levelTextView.Init(_levelData.NumberOfLevel);
            _player.Init();
        }

        public void RestartLevel()
        {
            _enemyContainer.ClearActiveEnemies();
            
            InitAll();
        }

        public void LoadNextLevel()
        {
            _progressData.IncreaseCurrentLevel();
            _enemyContainer.ClearActiveEnemies();
            _jsonLevelParser.FetchLevelSettings();
        }
    }
}