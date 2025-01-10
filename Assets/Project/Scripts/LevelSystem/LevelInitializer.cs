using System;
using Project.Architecture.UI;
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
using VContainer.Unity;
using YG;

namespace Project.LevelSystem
{
    public class LevelInitializer : IDisposable, IStartable
    {
        private EnemySpawner _enemySpawner;
        private GameOutcome _gameOutcome;
        private RewardAdHandler _rewardAdHandler;
        private Player _player;
        private LevelLoader _levelLoader;
        private SurviveTimerView _surviveTimerView;
        private EnemyContainer _enemyContainer;
        private LevelTextView _levelTextView;
        private PauseHandler _pauseHandler;
        private ProgressData _progressData;

        private WonPanel _wonPanel;
        private LosePanel _losePanel;

        private bool _isProgressDataSubscribed;

        public event Action LastLevelReached;

        private LevelData _levelData;

        [Inject]
        public void Construct(LevelLoader levelLoader, EnemySpawner enemySpawner, GameOutcome gameOutcome,
            RewardAdHandler rewardAdHandler, Player player, SurviveTimerView timersView, EnemyContainer enemyContainer,
            LevelTextView levelTextView, LosePanel losePanel, WonPanel wonPanel, PauseHandler pauseHandler,
            ProgressData progressData)
        {
            _levelLoader = levelLoader;
            _enemySpawner = enemySpawner;
            _gameOutcome = gameOutcome;
            _rewardAdHandler = rewardAdHandler;
            _player = player;
            _surviveTimerView = timersView;
            _enemyContainer = enemyContainer;
            _levelTextView = levelTextView;
            _pauseHandler = pauseHandler;
            _progressData = progressData;

            _wonPanel = wonPanel;
            _losePanel = losePanel;

            SubscribeEvents();
        }

        public void Start() { }

        public void Dispose()
        {
            UnsubscribeEvents();
        }

        private void LoadNextLevel()
        {
            if (_progressData.TryIncreaseCurrentLevel())
            {
                _enemyContainer.ClearActiveEnemies();
                _levelLoader.FetchCurrentLevelSettings();
                return;
            }

            MetricaSender.SendWithId(MetricaId.LastLevelReachedId, _progressData.CurrentLevelNumber.ToString());
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
            _losePanel.RestartRequested += RestartLevel;
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
            _player.Initialize();
            _rewardAdHandler.Initialize();
        }
    }
}