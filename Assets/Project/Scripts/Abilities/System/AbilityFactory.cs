using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using YG;

namespace Project.Abilities
{
    public class AbilityFactory : IAbilityFactory
    {
        private readonly IObjectResolver _container;

        private readonly AbilityInputManager _inputManager;
        private readonly AbilityKeyCodesBank _keyCodes;
        private int _keycodeIndex = 0;

        private readonly HashSet<Ability> _abilities = new();
        private readonly Dictionary<Type, Type> _abilityMap;

        public AbilityFactory(IObjectResolver container, AbilityInputManager inputManager, AbilityKeyCodesBank keyCodes)
        {
            _container = container;
            _inputManager = inputManager;
            _keyCodes = keyCodes;

            _abilityMap = new Dictionary<Type, Type>
            {
                { typeof(SlowModeAbilityData), typeof(SlowModeAbility) },
            };
        }

        public Ability CreateAbility(AbilityData data)
        {
            if (!_abilityMap.TryGetValue(data.GetType(), out Type abilityType))
            {
                Debug.LogError($"Ability type {data.GetType().Name} is not supported.");
                return null;
            }

            var ability = (Ability)_container.Resolve(abilityType);
            ability.SetData(data);
            _abilities.Add(ability);

            var activationKey = _keyCodes.GetKeycode(_keycodeIndex);

            if (activationKey != KeyCode.None)
            {
                var inputHandler = new AbilityKeyboardInput(ability, activationKey);
                _inputManager.Register(inputHandler);
                _keycodeIndex++;
            }
            else
            {
                Debug.LogError(
                    $"No available key codes for ability {data.AbilityName}. It will not have a keyboard shortcut.");
            }

            return ability;
        }

        public void Initialize()
        {
            foreach (Ability ability in _abilities)
            {
                ability.Deactivate();
                ability.Reload();
            }
        }

        public List<Ability> GetAllAbilities() => _abilities.ToList();

        public List<Ability> GetUnlockedAbilities() =>
            _abilities.Where(a => YandexGame.savesData.UnlockedAbilityNames.Contains(a.Data.AbilityName)).ToList();
    }
}