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

        public bool IsActive { get; protected set; }
        public AbilityData CommonData { get; private set; }
        protected bool IsReady => CooldownTimer.IsActive == false;

        protected abstract void OnActivate();
        protected abstract void OnDeactivate();

        public void Dispose()
        {
            OnDispose();
        }

        protected virtual void OnDispose() { }

        public void SetData(AbilityData data)
        {
            CommonData = data;
            CastData();
        }

        protected abstract void CastData();

        public void Start()
        {
            CooldownTimer.SetTime(CommonData.Cooldown);
            OnStart();
        }

        public virtual void OnStart() { }

        protected void InvokeOnActivated()
        {
            OnActivated?.Invoke();
        }

        protected void InvokeOnDeactivated()
        {
            OnDeactivated?.Invoke();
        }

        public virtual bool TryActivate()
        {
            if (IsActive || IsReady == false)
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
                CooldownTimer.Start(CommonData.Cooldown);
            }

            InvokeOnActivated();
            Debug.Log("Ability activated");
        }

        public void Reload()
        {
            CooldownTimer.ForceStop();
            CooldownTimer.SetTime(CommonData.Cooldown);
        }

        public void ReActivate()
        {
            if (IsActive)
            {
                Deactivate();
            }

            ForceActivate(false);
        }
    }
}