using System;
using UnityEngine;
using VavilichevGD.Utils.Timing;

namespace Project.Abilities
{
    public abstract class Ability : IHasCooldown
    {
        public readonly SyncedTimer CooldownTimer = new(TimerType.UpdateTick);

        public event Action OnActivated;
        public event Action OnDeactivated;

        public bool IsActive { get; private set; }

        protected abstract void OnActivate();
        protected abstract void OnDeactivate();
        public abstract float GetCooldown();

        public void ReActivate()
        {
            if (IsActive)
                Deactivate();

            ForceActivate();
        }

        public bool TryActivate()
        {
            if (IsActive || IsReady() == false)
                return false;

            ForceActivate();
            return true;
        }

        private void ForceActivate()
        {
            IsActive = true;
            OnActivate();
            OnActivated?.Invoke();

            Debug.LogError("Ability activated");
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;
            OnDeactivate();
            OnDeactivated?.Invoke();
            Debug.LogError("Ability deactivated");
        }

        public float GetRemainingCooldown()
        {
            return CooldownTimer.RemainingSeconds;
        }

        public bool IsReady()
        {
            return !CooldownTimer.IsActive;
        }
        
        public void Reload()
        {
            CooldownTimer.SetTime(GetCooldown());
        }
    }
}