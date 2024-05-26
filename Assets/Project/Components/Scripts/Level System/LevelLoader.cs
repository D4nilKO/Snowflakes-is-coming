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
        [SerializeField] private LevelSettings _levelSettings;
        [SerializeField] private TimersView _timersView;
        [SerializeField] private EnemyContainer _enemyContainer;
        
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
            _levelSettings.LevelSettingsReady += LoadLevelAfterSetSettings;
        }

        private void UnsubscribeEvents()
        {
            _levelSettings.LevelSettingsReady -= LoadLevelAfterSetSettings;
        }

        private void LoadLevelAfterSetSettings(LevelData levelData)
        {
            Debug.Log("load level");

            _enemySpawner.Init(levelData.EnemyTypesInfo, levelData.TimeToSpawn);
            _gameOutcome.Init(levelData.TimeToSurvive);
            _timersView.Init(levelData.TimeToSurvive);
            _player.Init();
            
            _levelData = levelData;
        }

        public void RestartLevel()
        {
            _enemyContainer.ClearActiveEnemies();
            _enemySpawner.Init(_levelData.EnemyTypesInfo, _levelData.TimeToSpawn);
            _gameOutcome.Init(_levelData.TimeToSurvive);
            _timersView.Init(_levelData.TimeToSurvive);
            _player.Init();
        }

        public void LoadNextLevel()
        {
            ProgressData.IncreaseCurrentLevel();
            _enemyContainer.ClearActiveEnemies();
            _levelSettings.SetLevelSettings();
        }
    }
}