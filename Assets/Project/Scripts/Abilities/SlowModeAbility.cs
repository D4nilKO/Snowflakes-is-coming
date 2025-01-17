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
        private SyncedTimer _timer = new(TimerType.UpdateTick);

        private SlowModeAbilityData _data;
        private EnemyContainer _enemyContainer;

        [Inject]
        public void Construct(EnemyContainer enemyContainer, SlowModeAbilityData slowModeData)
        {
            _enemyContainer = enemyContainer;
            _data = slowModeData;
        }

        public void Start()
        {
            SubscribeToTimer();
            _timer.SetTime(_data.Duration);
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

        public override void Activate()
        {
            Debug.LogError("Slow mode activated");
            _enemyContainer.GetEnemies().ForEach(enemy => enemy.SlowFactor = _data.SlowFactor);
            _enemyContainer.SetSlowFactor(_data.SlowFactor);
            _timer.Start(_data.Duration);
        }

        public override void Deactivate()
        {
            Debug.LogError("Slow mode deactivated");
            _enemyContainer.GetEnemies().ForEach(enemy => enemy.SlowFactor = 0f);
            _enemyContainer.SetSlowFactor(0f);
        }
    }
}