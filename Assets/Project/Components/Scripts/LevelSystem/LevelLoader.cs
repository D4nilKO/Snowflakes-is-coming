using Project.Data;
using Project.Entities.Character;
using Project.Entities.Enemies;
using Project.GameState;
using Project.GameState.View;
using Project.LevelSystem.LevelStructure;
using Project.Services;
using Project.Timing;
using UnityEngine;

namespace Project.LevelSystem
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField]
        private EnemySpawner _enemySpawner;

        [SerializeField]
        private GameOutcome _gameOutcome;

        [SerializeField]
        private RewardAdHandler _rewardAdHandler;

        [SerializeField]
        private Player _player;

        [SerializeField]
        private Game _game;

        [SerializeField]
        private TimersView _timersView;

        [SerializeField]
        private EnemyContainer _enemyContainer;

        [SerializeField]
        private LevelTextView _levelTextView;

        [SerializeField]
        private ProgressData _progressData;

        private LevelData _levelData;

        private void Awake()
        {
            this.ValidateSerializedFields();

            SubscribeEvents();
            Debug.Log("level loader awake");
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        public void LoadNextLevel()
        {
            _progressData.IncreaseCurrentLevel();
            _enemyContainer.ClearActiveEnemies();
            _game.FetchCurrentLevelSettings();
        }

        public void RestartLevel()
        {
            _enemyContainer.ClearActiveEnemies();

            InitializeLevel();
        }

        private void SubscribeEvents()
        {
            _game.LevelSettingsReady += SetLevelData;
        }

        private void UnsubscribeEvents()
        {
            _game.LevelSettingsReady -= SetLevelData;
        }

        private void SetLevelData(LevelData levelData)
        {
            _levelData = levelData;

            InitializeLevel();
        }

        private void InitializeLevel()
        {
            _enemySpawner.Initialize(_levelData.EnemyTypesInfo, _levelData.TimeToSpawn);
            _gameOutcome.Initialize(_levelData.GetTimeToSurvive());
            _timersView.Initialize(_levelData.GetTimeToSurvive());
            _levelTextView.Initialize(_levelData.NumberOfLevel);
            _player.Initialize();
            _rewardAdHandler.Initialize();
        }
    }
}