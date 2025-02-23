using UnityEngine;
using VContainer.Unity;

namespace Project.Abilities
{
    public class AbilityInputHandler : ITickable
    {
        private Ability _ability;
        private KeyCode _activationKey;

        public AbilityInputHandler(Ability ability, KeyCode activationKey)
        {
            _ability = ability;
            _activationKey = activationKey;
        }

        public void Tick()
        {
            HandleKeyboardInput();
        }

        private void HandleKeyboardInput()
        {
            if (_activationKey == KeyCode.None || _ability == null)
                return;

            if (Input.GetKeyDown(_activationKey))
                _ability.TryActivate();
        }
    }
}