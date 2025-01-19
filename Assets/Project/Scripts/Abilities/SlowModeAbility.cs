using System;
using Project.Entities.Enemies;
using UnityEngine;
using VavilichevGD.Utils.Timing;
using VContainer;
using VContainer.Unity;

namespace Project.Abilities
{
    public class SlowModeAbility : Ability, IStartable, IDisposable
    {
        private readonly SyncedTimer _timer = new(TimerType.UpdateTick);

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
            _timer.SetTime(_data.Duration);
        }

        public void Initialize()
        {
            Deactivate();
        }

        public void Dispose()
        {
            UnsubscribeFromTimer();
        }

        private void SubscribeToTimer()
        {
            _timer.TimerFinished += Deactivate;
        }

        private void UnsubscribeFromTimer()
        {
            _timer.TimerFinished -= Deactivate;
        }

        protected override void OnActivate()
        {
            _enemyContainer.GetEnemies().ForEach(enemy => enemy.SlowFactor = _data.SlowFactor);
            _enemyContainer.SetSlowFactor(_data.SlowFactor);
            _timer.Restart(_data.Duration);
        }

        protected override void OnDeactivate()
        {
            _enemyContainer.GetEnemies().ForEach(enemy => enemy.SlowFactor = 0f);
            _enemyContainer.SetSlowFactor(0f);
        }
    }
}