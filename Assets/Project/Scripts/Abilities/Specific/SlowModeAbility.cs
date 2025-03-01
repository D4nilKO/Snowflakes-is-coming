using Project.Entities.Enemies;
using VavilichevGD.Utils.Timing;
using VContainer;
using VContainer.Unity;

namespace Project.Abilities
{
    public class SlowModeAbility : Ability, IStartable
    {
        private readonly SyncedTimer _durationTimer = new(TimerType.UpdateTick);
        private EnemyContainer _enemyContainer;

        [Inject]
        public void Construct(EnemyContainer enemyContainer)
        {
            _enemyContainer = enemyContainer;
        }

        public void Start()
        {
            _durationTimer.SetTime(Data.Cooldown);
            _durationTimer.TimerFinished += Deactivate;
        }

        public override void Dispose()
        {
            _durationTimer.TimerFinished -= Deactivate;
        }

        protected override void OnActivate()
        {
            var slowData = (SlowModeAbilityData)Data;
            _enemyContainer.GetEnemies().ForEach(enemy => enemy.SlowFactor = slowData.SlowFactor);
            _enemyContainer.SetSlowFactor(slowData.SlowFactor);
            _durationTimer.Restart(slowData.Duration);
        }

        protected override void OnDeactivate()
        {
            _enemyContainer.GetEnemies().ForEach(enemy => enemy.SlowFactor = 0f);
            _enemyContainer.SetSlowFactor(0f);
        }
    }
}