using UnityEngine;

namespace Project.Architecture
{
    public class RootLifetimeScopeInitializer : MonoBehaviour
    {
        [SerializeField]
        private RootLifetimeScope _rootLifetimeScopePrefab;

        private bool IsRootLifetimeScopeInitialized => FindFirstObjectByType<RootLifetimeScope>() != null;

        private void Awake()
        {
            if (IsRootLifetimeScopeInitialized)
                return;

            Instantiate(_rootLifetimeScopePrefab);
        }
    }
}