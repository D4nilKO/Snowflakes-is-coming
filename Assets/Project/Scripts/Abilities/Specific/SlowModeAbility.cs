using Project.Entities.Enemies;
using VavilichevGD.Utils.Timing;
using VContainer;
using VContainer.Unity;

namespace Project.Abilities
{
    public class SlowModeAbility : Ability
    {
        private readonly SyncedTimer _durationTimer = new(TimerType.UpdateTick);
        private EnemyContainer _enemyContainer;
        private SlowModeAbilityData _data;

        [Inject]
        public void Construct(EnemyContainer enemyContainer)
        {
            _enemyContainer = enemyContainer;
        }

        protected override void CastData()
        {
            _data = (SlowModeAbilityData)CommonData;
        }

        public override void OnStart()
        {
            _durationTimer.SetTime(_data.Duration);
            _durationTimer.TimerFinished += Deactivate;
        }

        protected override void OnDispose()
        {
            _durationTimer.TimerFinished -= Deactivate;
        }

        protected override void OnActivate()
        {
            _enemyContainer.GetEnemies().ForEach(enemy => enemy.SlowFactor = _data.SlowFactor);
            _enemyContainer.SetSlowFactor(_data.SlowFactor);
            _durationTimer.Restart(_data.Duration);
        }

        protected override void OnDeactivate()
        {
            _enemyContainer.GetEnemies().ForEach(enemy => enemy.SlowFactor = 0f);
            _enemyContainer.SetSlowFactor(0f);
        }
    }
}