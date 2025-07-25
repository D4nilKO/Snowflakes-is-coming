using Project.Entities.Enemies;
using VContainer;

namespace Project.Abilities
{
    public class RemoveEnemyAbility : InstantAbility
    {
        private EnemyContainer _enemyContainer;
        private RemoveEnemyAbilityData _data;

        [Inject]
        public void Construct(EnemyContainer enemyContainer)
        {
            _enemyContainer = enemyContainer;
        }

        protected override void CastData()
        {
            _data = (RemoveEnemyAbilityData)CommonData;
        }

        protected override void OnActivate()
        {
            _enemyContainer.DespawnEnemiesAbility(_data.Count);
        }
    }
}