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
                Deactivate();

            IsActive = true;
            OnActivate();
            OnActivated?.Invoke();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;
            OnDeactivate();
            OnDeactivated?.Invoke();
        }

        protected abstract void OnActivate();
        protected abstract void OnDeactivate();
    }
}