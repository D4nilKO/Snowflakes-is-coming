using Project.Architecture.UI;
using Project.GameState;
using Project.GameState.View;
using Project.LevelSystem;
using Project.Menu;
using Project.Services;
using Project.Timing;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Architecture
{
    public class GameLifetimeScope : LifetimeScope
    {
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
        private DevPanelSwitcher _devPanelSwitcher;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentOnNewGameObject<LevelInfoView>(Lifetime.Scoped);

            builder.RegisterComponentInNewPrefab(_inGamePausePanel, Lifetime.Scoped);
            builder.RegisterComponentInNewPrefab(_trainingPanel, Lifetime.Scoped);
            builder.RegisterComponentInNewPrefab(_finalPanel, Lifetime.Scoped);
            builder.RegisterComponentInNewPrefab(_losePanel, Lifetime.Scoped);
            builder.RegisterComponentInNewPrefab(_wonPanel, Lifetime.Scoped);
            builder.RegisterComponentInNewPrefab(_devPanel, Lifetime.Scoped);

            builder.Register<Training>(Lifetime.Scoped);

            builder.Register<PauseHandler>(Lifetime.Scoped);
            builder.Register<GameOutcome>(Lifetime.Scoped);
            builder.Register<GameOutcomeView>(Lifetime.Scoped);

            builder.Register<LevelInitializer>(Lifetime.Scoped);
            builder.Register<LevelLoader>(Lifetime.Scoped);
            builder.Register<SceneLoader>(Lifetime.Singleton);

            builder.RegisterComponentInHierarchy<LevelTextView>();
            builder.RegisterComponentInHierarchy<JsonLevelParser>();
            builder.RegisterComponentInHierarchy<DevPanel>();
        }
    }
}