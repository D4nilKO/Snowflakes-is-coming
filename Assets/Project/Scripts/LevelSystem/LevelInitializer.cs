using System;
using Project.Data;
using Project.Entities.Character;
using Project.Entities.Enemies;
using Project.GameState;
using Project.GameState.View;
using Project.LevelSystem.LevelStructure;
using Project.Services;
using Project.Timing;
using UnityEngine.EventSystems;
using VContainer;

namespace Project.LevelSystem
{
    public class LevelInitializer : IDisposable
    {
        private EnemySpawner _enemySpawner;
        private GameOutcome _gameOutcome;
        private RewardAdHandler _rewardAdHandler;
        private Player _player;
        private LevelLoader _levelLoader;
        private TimersView _timersView;
        private EnemyContainer _enemyContainer;
        private LevelTextView _levelTextView;

        public event Action LastLevelReached;

        private LevelData _levelData;

        [Inject]
        public void Construct(LevelLoader levelLoader, EnemySpawner enemySpawner, GameOutcome gameOutcome,
            RewardAdHandler rewardAdHandler, Player player, TimersView timersView, EnemyContainer enemyContainer,
            LevelTextView levelTextView)
        {
            _levelLoader = levelLoader;
            _enemySpawner = enemySpawner;
            _gameOutcome = gameOutcome;
            _rewardAdHandler = rewardAdHandler;
            _player = player;
            _timersView = timersView;
            _enemyContainer = enemyContainer;
            _levelTextView = levelTextView;

            SubscribeEvents();
        }

        public void Dispose()
        {
            UnsubscribeEvents();
        }

        public void LoadNextLevel()
        {
            if (ProgressData.TryIncreaseCurrentLevel())
            {
                _enemyContainer.ClearActiveEnemies();
                _levelLoader.FetchCurrentLevelSettings();
                return;
            }

            MetricaSender.SendWithId(MetricaId.LastLevelReachedId, ProgressData.CurrentLevelNumber.ToString());
            LastLevelReached?.Invoke();
        }

        public void RestartLevel()
        {
            _enemyContainer.ClearActiveEnemies();
            EventSystem.current.SetSelectedGameObject(null);

            InitializeLevel();
        }

        private void SubscribeEvents()
        {
            _levelLoader.LevelSettingsReady += SetLevelData;
        }

        private void UnsubscribeEvents()
        {
            _levelLoader.LevelSettingsReady -= SetLevelData;
        }

        private void SetLevelData(LevelData levelData)
        {
            _levelData = levelData;

            InitializeLevel();
        }

        private void InitializeLevel()
        {
            InitializeComponents();

            MetricaSender.SendWithId(MetricaId.LevelStartId, _levelData.NumberOfLevel.ToString());
        }

        private void InitializeComponents()
        {
            int timeToSurvive = _levelData.GetTimeToSurvive();
            
            _enemySpawner.Initialize(_levelData.EnemyTypesInfo, _levelData.TimeToSpawn);
            _gameOutcome.Initialize(timeToSurvive);
            _timersView.Initialize(timeToSurvive);
            _levelTextView.Initialize(_levelData.NumberOfLevel);
            _player.Initialize();
            _rewardAdHandler.Initialize();
        }
    }
}