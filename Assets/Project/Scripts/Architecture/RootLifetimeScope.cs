using Project.Data;
using Project.LevelSystem;
using Project.Menu;
using Project.Utility;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Architecture
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField, Footer("----------------")]
        private JsonLevelParser _jsonLevelParser;

        protected override void Configure(IContainerBuilder builder)
        {
            ConfigureSingletons(builder);
            DontDestroyOnLoad(gameObject);

            Debug.Log("RootLifetimeScope configure");
        }

        private void ConfigureSingletons(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<ProgressData>().AsSelf();
            builder.Register<SceneLoader>(Lifetime.Singleton);

            if (FindFirstObjectByType<JsonLevelParser>() == null)
            {
                var jsonLevelParserInstance = Instantiate(_jsonLevelParser);
                DontDestroyOnLoad(jsonLevelParserInstance.gameObject);
                builder.RegisterComponent(jsonLevelParserInstance);
            }
            else
            {
                builder.RegisterComponent(FindFirstObjectByType<JsonLevelParser>());
            }
        }
    }
}