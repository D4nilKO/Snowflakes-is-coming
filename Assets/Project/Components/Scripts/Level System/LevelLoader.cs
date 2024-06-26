﻿using Project.Components.Scripts.Data;
using Project.Components.Scripts.Entities.Character;
using Project.Components.Scripts.Entities.Enemies;
using Project.Components.Scripts.GameState;
using Project.Components.Scripts.GameState.View;
using Project.Components.Scripts.Level_System.LevelStructure;
using Project.Components.Scripts.Timing;
using UnityEngine;

namespace Project.Components.Scripts.Level_System
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private GameOutcome _gameOutcome;
        [SerializeField] private Player _player;
        [SerializeField] private Game _game;
        [SerializeField] private TimersView _timersView;
        [SerializeField] private EnemyContainer _enemyContainer;
        [SerializeField] private LevelTextView _levelTextView;
        [SerializeField] private ProgressData _progressData;

        private LevelData _levelData;

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
        }
    }
}