using Project.Architecture;
using UnityEngine;

public class RootLifetimeScopeInitializer : MonoBehaviour
{
    [SerializeField]
    private RootLifetimeScope _rootLifetimeScopePrefab;

    private static bool _isRootLifetimeScopeInitialized; 

    private void Awake()
    {
        if (!_isRootLifetimeScopeInitialized)
        {
            Instantiate(_rootLifetimeScopePrefab);
            _isRootLifetimeScopeInitialized = true;
        }
    }
}