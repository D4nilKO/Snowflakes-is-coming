using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Project.Abilities
{
    public class AbilityInputRegistry : MonoBehaviour
    {
        private readonly List<AbilityKeyboardInput> _inputs = new();

        private AbilityKeyCodesBank _keyCodes;
        private IAbilityFactory _abilityFactory;

        [Inject]
        public void Construct(AbilityKeyCodesBank keyCodesBank, IAbilityFactory abilityFactory)
        {
            _keyCodes = keyCodesBank;
            _abilityFactory = abilityFactory;
        }

        private void Update()
        {
            foreach (var input in _inputs)
            {
                input.Tick();
            }
        }

        public void Register(AbilityKeyboardInput input)
        {
            if (_inputs.Contains(input) == false)
                _inputs.Add(input);
        }

        public KeyCode GetAbilityKey(Ability ability)
        {
            if (ability == null)
                return KeyCode.None;

            return _keyCodes.Get(_abilityFactory.GetAll().IndexOf(ability));
        }
    }
}