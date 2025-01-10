using Project.LevelSystem;
using UnityEngine;
using VContainer;

namespace Project.Menu
{
    [RequireComponent(typeof(MenuLifetimeScope))]
    public class MenuResolver : MonoBehaviour
    {
        private IObjectResolver _container;

        private void Start()
        {
            Debug.Log("MenuResolver Start");

            var lifetimeScope = GetComponent<MenuLifetimeScope>();
            _container = lifetimeScope.Container;

            if (_container == null)
            {
                Debug.LogError("MenuResolver container is null");
            }

            _container.Resolve<LevelSelector>();
            _container.Resolve<MainMenuPanel>();
            _container.Resolve<JsonLevelParser>();
        }
    }
}