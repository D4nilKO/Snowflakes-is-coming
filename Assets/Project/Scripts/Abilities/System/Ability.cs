using System;
using UnityEngine;
using VavilichevGD.Utils.Timing;

namespace Project.Abilities
{
    public abstract class Ability : IDisposable
    {
        public readonly SyncedTimer CooldownTimer = new(TimerType.UpdateTick);

        public event Action OnActivated;
        public event Action OnDeactivated;

        public bool IsActive { get; private set; }
        public AbilityData Data { get; private set; }

        protected abstract void OnActivate();
        protected abstract void OnDeactivate();

        public void SetData(AbilityData data)
        {
            Data = data;
            CooldownTimer.SetTime(data.Cooldown);
        }

        public bool TryActivate()
        {
            if (IsActive || IsReady() == false)
            {
                Debug.LogWarning($"Ability {GetType().Name} is not ready to activate");
                return false;
            }

            ForceActivate();
            return true;
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;
            OnDeactivate();
            OnDeactivated?.Invoke();
            Debug.Log("Ability deactivated");
        }

        private void ForceActivate(bool startCooldown = true)
        {
            IsActive = true;
            OnActivate();

            if (startCooldown)
            {
                CooldownTimer.Start(Data.Cooldown);
            }

            OnActivated?.Invoke();
            Debug.Log("Ability activated");
        }

        public float GetRemainingCooldown() => CooldownTimer.RemainingSeconds;
        public bool IsReady() => !CooldownTimer.IsActive;

        public void Reload()
        {
            CooldownTimer.ForceStop();
            CooldownTimer.SetTime(Data.Cooldown);
        }

        public void ReActivate()
        {
            if (IsActive)
            {
                Deactivate();
            }

            ForceActivate(false);
        }

        public virtual void Dispose() { }
    }
}