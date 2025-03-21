using UnityEngine;

namespace Project.Abilities
{
    public abstract class InstantAbility : Ability
    {
        public override bool TryActivate()
        {
            if (IsReady == false)
            {
                Debug.LogWarning($"Ability {GetType().Name} is not ready to activate");
                return false;
            }

            ForceActivate();
            return true;
        }

        public override void Deactivate() { }
        protected override void OnDeactivate() { }
    }
}