using System;
using Project.Entities.Enemies;
using VavilichevGD.Utils.Timing;
using VContainer;
using VContainer.Unity;

namespace Project.Abilities
{
    public class SlowModeAbility : Ability, IStartable
    {
        private readonly SyncedTimer _durationTimer = new(TimerType.UpdateTick);

        private SlowModeAbilityData _data;
        private EnemyContainer _enemyContainer;

        [Inject]
        public void Construct(EnemyContainer enemyContainer)
        {
            _enemyContainer = enemyContainer;
        }

        public void SetData(SlowModeAbilityData data)
        {
            _data = data;
        }

        public void Start()
        {
            SubscribeToTimer();
            _durationTimer.SetTime(_data.Duration);
            CooldownTimer.SetTime(_data.Cooldown);
        }

        public override void Dispose()
        {
            UnsubscribeFromTimer();
        }

        private void SubscribeToTimer()
        {
            _durationTimer.TimerFinished += Deactivate;
        }

        private void UnsubscribeFromTimer()
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

        public override float GetCooldown()
        {
            return _data.Cooldown + _data.Duration;
        }

        public override AbilityData GetData()
        {
            return _data;
        }
    }
}