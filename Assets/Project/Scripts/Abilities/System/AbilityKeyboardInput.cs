using UnityEngine;

namespace Project.Abilities
{
    public class AbilityKeyboardInput
    {
        private readonly Ability _ability;
        private readonly KeyCode _activationKey;

        public AbilityKeyboardInput(Ability ability, KeyCode activationKey)
        {
            _ability = ability;
            _activationKey = activationKey;
        }

        public void Tick()
        {
            if (_activationKey == KeyCode.None || _ability == null)
                return;

            if (Input.GetKeyDown(_activationKey))
                _ability.TryActivate();
        }
    }
}