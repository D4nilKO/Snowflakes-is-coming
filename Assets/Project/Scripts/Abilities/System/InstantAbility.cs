using UnityEngine;

namespace Project.Abilities
{
    public abstract class InstantAbility : Ability
    {
        public override bool TryActivate()
        {
            if (IsReady() == false)
            {
                Debug.LogWarning($"Ability {GetType().Name} is not ready to activate");
                return false;
            }

            OnActivate();
            InvokeOnActivated();
            Debug.Log("Instant ability activated");
            return true;
        }

        public override void Deactivate() { }

        public override void Reload() { }
    }
}