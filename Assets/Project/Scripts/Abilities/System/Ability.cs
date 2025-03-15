using System;
using UnityEngine;
using VavilichevGD.Utils.Timing;

namespace Project.Abilities
{
    public abstract class Ability : IDisposable
    {
        public event Action OnActivated;
        public event Action OnDeactivated;

        public bool IsActive { get; protected set; }
        public AbilityData Data { get; private set; }

        public readonly SyncedTimer CooldownTimer = new(TimerType.UpdateTick);

        public Ability()
        {
            CooldownTimer.TimerFinished += Deactivate;
        }

        public abstract void Dispose();

        protected abstract void OnActivate();

        protected abstract void OnDeactivate();

        public void SetData(AbilityData data)
        {
            Data = data;
        }

        protected void InvokeOnActivated()
        {
            OnActivated?.Invoke();
        }

        protected void InvokeOnDeactivated()
        {
            OnDeactivated?.Invoke();
        }

        public float GetRemainingCooldown() => CooldownTimer.RemainingSeconds;
        protected bool IsReady() => CooldownTimer.IsActive == false;

        public virtual bool TryActivate()
        {
            if (IsActive || IsReady() == false)
            {
                Debug.LogWarning($"Ability {GetType().Name} is not ready to activate");
                return false;
            }

            ForceActivate();
            return true;
        }

        public virtual void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;
            OnDeactivate();
            InvokeOnDeactivated();
            Debug.Log("Ability deactivated");
        }

        protected void ForceActivate(bool startCooldown = true)
        {
            IsActive = true;
            OnActivate();

            if (startCooldown)
            {
                CooldownTimer.Start(Data.Cooldown);
            }

            InvokeOnActivated();
            Debug.Log("Ability activated");
        }

        public virtual void Reload()
        {
            CooldownTimer.ForceStop();
            CooldownTimer.SetTime(Data.Cooldown);
        }

        public void ReActivate()
        {
            if (IsActive)
                Deactivate();

            ForceActivate(false);
        }
    }
}