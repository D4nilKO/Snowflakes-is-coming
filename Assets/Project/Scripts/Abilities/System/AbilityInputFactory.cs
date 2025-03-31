using System;
using System.Collections.Generic;
using Project.Data.Collections;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Abilities
{
    public class AbilityInputFactory : IStartable, IDisposable
    {
        private AbilityKeyCodesBank _keyCodes;
        private AbilityInputRegistry _inputRegistry;
        private IAbilityFactory _abilityFactory;
        private CollectionManager _collectionManager;

        private int _keycodeIndex;

        [Inject]
        public AbilityInputFactory(AbilityKeyCodesBank keyCodes, AbilityInputRegistry inputRegistry,
            IAbilityFactory abilityFactory, CollectionManager collectionManager)
        {
            _keyCodes = keyCodes;
            _inputRegistry = inputRegistry;
            _abilityFactory = abilityFactory;
            _collectionManager = collectionManager;
        }

        public void Start()
        {
            List<Ability> unlockedAbilities = _abilityFactory.GetAllUnlocked();
            CreateInputs(unlockedAbilities);

            _collectionManager.OnCollectionCompleted += Create;
        }

        public void Dispose()
        {
            _collectionManager.OnCollectionCompleted -= Create;
        }

        private void CreateInputs(List<Ability> abilities)
        {
            foreach (var ability in abilities)
            {
                Create(ability);
            }
        }

        private void Create(Ability ability)
        {
            var activationKey = _keyCodes.Get(_keycodeIndex);

            if (activationKey != KeyCode.None)
            {
                var inputHandler = new AbilityKeyboardInput(ability, activationKey);
                _inputRegistry.Register(inputHandler);
                _keycodeIndex++;
            }
            else
            {
                Debug.LogError(
                    $"No available key codes for ability {ability.CommonData.AbilityName}. It will not have a keyboard shortcut.");
            }
        }
    }
}