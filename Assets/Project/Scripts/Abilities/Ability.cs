using System;

namespace Project.Abilities
{
    public abstract class Ability
    {
        public event Action OnActivated;
        public event Action OnDeactivated;

        public bool IsActive { get; private set; }

        public void Activate()
        {
            if (IsActive)
                return;

            IsActive = true;
            OnActivate();
            OnActivated?.Invoke();
        }

        public void Deactivate()
        {
            if (!IsActive)
                return;

            IsActive = false;
            OnDeactivate();
            OnDeactivated?.Invoke();
        }

        protected abstract void OnActivate();
        protected abstract void OnDeactivate();
    }
}