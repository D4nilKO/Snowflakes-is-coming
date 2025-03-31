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
using Project.NewPhysics;
using Project.Services;
using Project.Services.MessageSystem;
using Project.Settings;
using Project.Timing;
using Project.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Architecture
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("Settings")]
        [SerializeField]
        private DevPanelSettings _devPanelSettings;

        [SerializeField]
        private AbilityKeyCodesBank _abilityKeyCodes;

        [Header("UI")]
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
        private ScreenMessageHandler _screenMessageHandler;

        [SerializeField]
        private SurviveTimerView _surviveTimerPanel;

        [Header("Gameplay")]
        [SerializeField]
        private Player _player;

        [SerializeField]
        private EnemyContainer _enemyContainer;

        [SerializeField]
        private EnemySpawner _enemySpawner;

        [SerializeField]
        private ScreenBounds _screenBounds;

        [Header("Parents")]
        [SerializeField]
        private Transform _mainCanvas;

        [SerializeField]
        private Transform _gamePlayParent;

        [SerializeField]
        private Transform _serviceParent;

        [SerializeField, Header("Snowflake Collections")]
        private List<CollectionData> _collections;

        protected override void Awake()
        {
            base.Awake();

            this.ValidateSerializedFields();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterCollections(builder);
            RegisterUI(builder);
            RegisterAbilities(builder);
            RegisterAbilityEnvironment(builder);

            var player = Instantiate(_player, _gamePlayParent);
            builder.RegisterComponent(player);
            builder.RegisterComponent(player.GetComponent<CharacterCollisionHandler>());

            builder.RegisterComponentInNewPrefab(_enemyContainer, Lifetime.Scoped).UnderTransform(_gamePlayParent);
            builder.RegisterComponentInNewPrefab(_screenBounds, Lifetime.Scoped).UnderTransform(_gamePlayParent);
            builder.RegisterComponentInNewPrefab(_enemySpawner, Lifetime.Scoped);

            builder.RegisterComponentOnNewGameObject<EntityRotator>(Lifetime.Scoped).UnderTransform(_gamePlayParent);

            builder.RegisterComponentOnNewGameObject<ScreenResizeHandler>(Lifetime.Scoped)
                .UnderTransform(_gamePlayParent);

            builder.RegisterComponentOnNewGameObject<LevelDataInfoView>(Lifetime.Scoped).UnderTransform(_serviceParent);

            builder.RegisterComponentOnNewGameObject<AbilityInputRegistry>(Lifetime.Scoped)
                .UnderTransform(_serviceParent);

            builder.RegisterInstance(_devPanelSettings);
            builder.RegisterInstance(_abilityKeyCodes);

            builder.RegisterEntryPoint<Training>(Lifetime.Scoped);
            builder.RegisterEntryPoint<DevPanelSwitcher>(Lifetime.Scoped);

            builder.Register<LevelInitializer>(Lifetime.Scoped);

            builder.Register<PauseHandler>(Lifetime.Scoped);

            builder.Register<GameOutcome>(Lifetime.Scoped);
            builder.Register<GameOutcomeView>(Lifetime.Scoped);

            builder.Register<LevelLoader>(Lifetime.Scoped);
            builder.Register<RewardAdHandler>(Lifetime.Scoped);
        }

        private static void RegisterAbilityEnvironment(IContainerBuilder builder)
        {
            builder.Register<CollectionManager>(Lifetime.Singleton);
            builder.Register<AbilityFactory>(Lifetime.Singleton).As<IAbilityFactory>();
            builder.RegisterEntryPoint<AbilityInputFactory>();
        }

        private void RegisterCollections(IContainerBuilder builder)
        {
            builder.RegisterBuildCallback(container =>
            {
                var collectionManager = container.Resolve<CollectionManager>();

                Debug.Log($"RegisterCollections: всего коллекций {_collections.Count}");

                foreach (CollectionData collection in _collections)
                {
                    if (collection.UnlockableAbilityData == null)
                    {
                        Debug.LogError($"Ошибка! У коллекции {collection.name} нет UnlockableAbilityData");
                        continue;
                    }

                    Debug.Log($"Регистрируем коллекцию: {collection.name}");
                    collectionManager.RegisterCollection(collection);
                }
            });
        }

        private void RegisterAbilities(IContainerBuilder builder)
        {
            builder.Register<SlowModeAbility>(Lifetime.Scoped);
            builder.Register<RemoveEnemyAbility>(Lifetime.Scoped);
        }

        private void RegisterUI(IContainerBuilder builder)
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
            builder.RegisterComponentInNewPrefab(_screenMessageHandler, Lifetime.Scoped).UnderTransform(_mainCanvas);

            builder.RegisterComponentInNewPrefab(_screenMessageHandler, Lifetime.Scoped).UnderTransform(_mainCanvas)
                .As<IScreenMessageHandler>();
        }
    }
}