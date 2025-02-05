using System.Collections.Generic;
using Project.Abilities;
using Project.Architecture.UI;
using Project.Data.Collections;
using Project.Entities;
using Project.Entities.Character;
using Project.Entities.Enemies;
using Project.GameState;
using Project.GameState.View;
using Project.LevelSystem;
using Project.Services;
using Project.Settings;
using Project.Timing;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Architecture
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private DevPanelSettings _devPanelSettings;

        [SerializeField]
        private InGamePausePanel _inGamePausePanel;

        [SerializeField]
        private TrainingPanel _trainingPanel;

        [SerializeField]
        private FinalPanel _finalPanel;

        [SerializeField]
        private LosePanel _losePanel;

        [SerializeField]
        private WonPanel _wonPanel;

        [SerializeField]
        private DevPanel _devPanel;

        [SerializeField]
        private LevelTextView _levelPanel;
        
        [SerializeField]
        private AbilityPanel _abilityPanel;

        [SerializeField]
        private SurviveTimerView _surviveTimerPanel;

        [SerializeField]
        private Player _player;

        [SerializeField]
        private EnemyContainer _enemyContainer;

        [SerializeField]
        private EnemySpawner _enemySpawner;

        [SerializeField]
        private Transform _mainCanvas;

        [SerializeField]
        private Transform _gamePlayParent;

        [SerializeField, Header("Snowflake Collections")]
        private List<CollectionData> _collections;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab(_inGamePausePanel, Lifetime.Scoped).UnderTransform(_mainCanvas);
            builder.RegisterComponentInNewPrefab(_trainingPanel, Lifetime.Scoped).UnderTransform(_mainCanvas);
            builder.RegisterComponentInNewPrefab(_finalPanel, Lifetime.Scoped).UnderTransform(_mainCanvas);
            builder.RegisterComponentInNewPrefab(_losePanel, Lifetime.Scoped).UnderTransform(_mainCanvas);
            builder.RegisterComponentInNewPrefab(_wonPanel, Lifetime.Scoped).UnderTransform(_mainCanvas);
            builder.RegisterComponentInNewPrefab(_devPanel, Lifetime.Scoped).UnderTransform(_mainCanvas);
            builder.RegisterComponentInNewPrefab(_levelPanel, Lifetime.Scoped).UnderTransform(_mainCanvas);
            builder.RegisterComponentInNewPrefab(_surviveTimerPanel, Lifetime.Scoped).UnderTransform(_mainCanvas);
            builder.RegisterComponentInNewPrefab(_abilityPanel, Lifetime.Scoped).UnderTransform(_mainCanvas);

            builder.RegisterComponentInNewPrefab(_player, Lifetime.Scoped).UnderTransform(_gamePlayParent);

            var player = Instantiate(_player);
            builder.RegisterComponent(player);
            builder.RegisterComponent(player.GetComponent<CharacterCollisionHandler>());

            builder.RegisterComponentInNewPrefab(_enemyContainer, Lifetime.Scoped).UnderTransform(_gamePlayParent);
            builder.RegisterComponentInNewPrefab(_enemySpawner, Lifetime.Scoped);

            builder.RegisterComponentOnNewGameObject<EntityRotator>(Lifetime.Scoped).UnderTransform(_gamePlayParent);
            builder.RegisterComponentOnNewGameObject<LevelDataInfoView>(Lifetime.Scoped);

            builder.RegisterInstance(_devPanelSettings);

            builder.RegisterEntryPoint<Training>(Lifetime.Scoped);
            builder.RegisterEntryPoint<DevPanelSwitcher>(Lifetime.Scoped);
            builder.RegisterEntryPoint<LevelInitializer>(Lifetime.Scoped).AsSelf();

            builder.Register<PauseHandler>(Lifetime.Scoped);

            builder.Register<GameOutcome>(Lifetime.Scoped);
            builder.Register<GameOutcomeView>(Lifetime.Scoped);

            builder.Register<LevelLoader>(Lifetime.Scoped);
            builder.Register<RewardAdHandler>(Lifetime.Scoped);

            RegisterCollections(builder);
        }

        private void RegisterCollections(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<SlowModeAbility>().AsSelf();
            builder.Register<AbilityFactory>(Lifetime.Singleton).As<IAbilityFactory>();
            builder.Register<CollectionManager>(Lifetime.Singleton);

            builder.RegisterBuildCallback(container =>
            {
                var collectionManager = container.Resolve<CollectionManager>();

                foreach (var collection in _collections)
                {
                    collectionManager.RegisterCollection(collection);
                }
            });
        }
    }
}