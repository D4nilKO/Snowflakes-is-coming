using Project.WindowSystem;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Menu
{
    public class MenuLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private LevelSelector _levelSelector;

        [SerializeField]
        private MainMenuPanel _mainMenuPanel;

        [SerializeField]
        private Transform _canvas;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<MenuEntryPoint>();
            builder.RegisterEntryPoint<WindowManager>().AsSelf();

            builder.RegisterComponentInNewPrefab(_levelSelector, Lifetime.Scoped).UnderTransform(_canvas);

            builder.RegisterComponentInNewPrefab(_mainMenuPanel, Lifetime.Scoped).UnderTransform(_canvas)
                .AsImplementedInterfaces();
        }
    }
}