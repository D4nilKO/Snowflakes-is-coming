﻿using System;
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

        public event Action LastLevelReached;

        private LevelData _levelData;

        private void Start()
        {
            this.ValidateSerializedFields();

            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        public void LoadNextLevel()
        {
            if (ProgressData.TryIncreaseCurrentLevel())
            {
                _enemyContainer.ClearActiveEnemies();
                _game.FetchCurrentLevelSettings();
                return;
            }

            MetricaSender.SendWithId(MetricaId.LastLevelReachedId, ProgressData.CurrentLevelNumber.ToString());
            LastLevelReached?.Invoke();
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

            MetricaSender.SendWithId(MetricaId.LevelStartId, _levelData.NumberOfLevel.ToString());
        }
    }
}