using Project.Abilities;
using Project.Architecture.UI;
using Project.Entities.Character;
using Project.Entities.Enemies;
using Project.GameState;
using Project.GameState.View;
using Project.Services;
using Project.Timing;
using UnityEngine;
using VContainer;

namespace Project.Architecture
{
    [RequireComponent(typeof(GameLifetimeScope))]
    public class GameResolver : MonoBehaviour
    {
        private IObjectResolver _container;

        private void Start()
        {
            Debug.Log("GameResolver Start");

            var lifetimeScope = GetComponent<GameLifetimeScope>();
            _container = lifetimeScope.Container;

            SpawnUIElements();
            SpawnGameplayElements();
        }

        private void SpawnUIElements()
        {
            _container.Resolve<InGamePausePanel>();
            _container.Resolve<TrainingPanel>();
            _container.Resolve<FinalPanel>();
            _container.Resolve<LosePanel>();
            _container.Resolve<WonPanel>();
            _container.Resolve<DevPanel>();
            _container.Resolve<AbilityPanel>();
            _container.Resolve<LevelTextView>();
            _container.Resolve<SurviveTimerView>();
        }

        private void SpawnGameplayElements()
        {
            _container.Resolve<Player>();
            _container.Resolve<LevelDataInfoView>();
            _container.Resolve<EnemyContainer>();
            _container.Resolve<EnemySpawner>();
            _container.Resolve<GameOutcomeView>();
        }
    }
}