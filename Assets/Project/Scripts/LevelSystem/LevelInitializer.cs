using System;
using Project.Abilities;
using Project.Architecture.UI;
using Project.Data;
using Project.Entities.Enemies;
using Project.GameState;
using Project.GameState.View;
using Project.LevelSystem.LevelStructure;
using Project.Services;
using Project.Timing;
using Project.UI;
using UnityEngine.EventSystems;
using VContainer;
using YG;

namespace Project.LevelSystem
{
    public class LevelInitializer : IDisposable
    {
        private EnemySpawner _enemySpawner;
        private GameOutcome _gameOutcome;
        private RewardAdHandler _rewardAdHandler;
        private LevelLoader _levelLoader;
        private SurviveTimerView _surviveTimerView;
        private EnemyContainer _enemyContainer;
        private LevelTextView _levelTextView;
        private PauseHandler _pauseHandler;
        private ProgressData _progressData;

        private AbilityPanel _abilityPanel;
        private IAbilityFactory _abilityFactory;

        private WonPanel _wonPanel;
        private LosePanel _losePanel;

        private bool _isProgressDataSubscribed;

        public event Action LastLevelReached;

        private LevelData _levelData;

        [Inject]
        public void Construct(LevelLoader levelLoader, EnemySpawner enemySpawner, GameOutcome gameOutcome,
            RewardAdHandler rewardAdHandler, SurviveTimerView timersView, EnemyContainer enemyContainer,
            LevelTextView levelTextView, LosePanel losePanel, WonPanel wonPanel, PauseHandler pauseHandler,
            ProgressData progressData, IAbilityFactory abilityFactory, AbilityPanel abilityPanel)
        {
            _levelLoader = levelLoader;
            _enemySpawner = enemySpawner;
            _gameOutcome = gameOutcome;
            _rewardAdHandler = rewardAdHandler;
            _surviveTimerView = timersView;
            _enemyContainer = enemyContainer;
            _levelTextView = levelTextView;
            _pauseHandler = pauseHandler;
            _progressData = progressData;

            _abilityFactory = abilityFactory;
            _abilityPanel = abilityPanel;

            _wonPanel = wonPanel;
            _losePanel = losePanel;

            SubscribeEvents();
        }

        public void Dispose()
        {
            UnsubscribeEvents();
        }

        private void LoadNextLevel()
        {
            if (_progressData.TryIncreaseCurrentLevel())
            {
                _enemyContainer.DespawnActiveEnemies();
                _levelLoader.FetchCurrentLevelSettings();
                return;
            }

            MetricaSender.SendWithId(MetricaId.LastLevelReachedId, _progressData.CurrentLevelNumber.ToString());
            LastLevelReached?.Invoke();
        }

        public void RestartLevel()
        {
            _enemyContainer.DespawnActiveEnemies();
            EventSystem.current.SetSelectedGameObject(null);

            InitializeLevel();
        }

        private void SubscribeEvents()
        {
            _levelLoader.LevelSettingsReady += SetLevelData;
            _losePanel.RestartRequested += RestartLevel;
            _wonPanel.RestartRequested += RestartLevel;
            _wonPanel.LoadNextLevelRequested += LoadNextLevel;

            if (YandexGame.SDKEnabled)
            {
                _levelLoader.FetchCurrentLevelSettings();
            }
            else
            {
                _isProgressDataSubscribed = true;
                YandexGame.GetDataEvent += _levelLoader.FetchCurrentLevelSettings;
            }
        }

        private void UnsubscribeEvents()
        {
            _levelLoader.LevelSettingsReady -= SetLevelData;
            _losePanel.RestartRequested -= RestartLevel;
            _wonPanel.RestartRequested -= RestartLevel;
            _wonPanel.LoadNextLevelRequested -= LoadNextLevel;

            if (_isProgressDataSubscribed)
            {
                YandexGame.GetDataEvent -= _levelLoader.FetchCurrentLevelSettings;
            }
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

            _pauseHandler.InGamePause();
            _enemySpawner.Initialize(_levelData.EnemyTypesInfo, _levelData.TimeToSpawn);
            _gameOutcome.Initialize(timeToSurvive);
            _surviveTimerView.Initialize(timeToSurvive);
            _levelTextView.Initialize(_levelData.NumberOfLevel);
            _rewardAdHandler.Initialize();
            _abilityFactory.Initialize();
            _abilityPanel.Initialize();
        }
    }
}