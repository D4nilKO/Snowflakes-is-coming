using System;
using UnityEngine;
using VavilichevGD.Utils.Timing;

namespace Project.Abilities
{
    public abstract class Ability : IHasCooldown, IDisposable
    {
        public readonly SyncedTimer CooldownTimer = new(TimerType.UpdateTick);

        public event Action OnActivated;
        public event Action OnDeactivated;

        public bool IsActive { get; private set; }

        protected abstract void OnActivate();
        protected abstract void OnDeactivate();
        public abstract float GetCooldown();
        public abstract AbilityData GetData();

        public void ReActivate()
        {
            if (IsActive)
                Deactivate();

            ForceActivate(false);
        }

        public bool TryActivate()
        {
            if (IsActive || IsReady() == false)
            {
                Debug.LogError($"Ability {GetType().Name} is not activate");
                return false;
            }

            ForceActivate();

            return true;
        }

        private void ForceActivate(bool startCooldown = true)
        {
            IsActive = true;
            OnActivate();

            if (startCooldown)
            {
                CooldownTimer.Start(GetCooldown());
            }

            OnActivated?.Invoke();

            Debug.Log("Ability activated");
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
            CooldownTimer.ForceStop();
            CooldownTimer.Stop();
            CooldownTimer.SetTime(GetCooldown());
            
        }

        public virtual void Dispose() { }
    }
}