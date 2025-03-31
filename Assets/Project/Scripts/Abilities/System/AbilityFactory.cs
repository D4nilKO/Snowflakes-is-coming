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

        private readonly HashSet<Ability> _abilities = new();
        private readonly Dictionary<Type, Type> _abilityMap;

        public List<Ability> GetAll() => _abilities.ToList();

        public AbilityFactory(IObjectResolver container)
        {
            _container = container;

            _abilityMap = new Dictionary<Type, Type>
            {
                { typeof(SlowModeAbilityData), typeof(SlowModeAbility) },
                { typeof(RemoveEnemyAbilityData), typeof(RemoveEnemyAbility) },
            };
        }

        public Ability Create(AbilityData data)
        {
            if (!_abilityMap.TryGetValue(data.GetType(), out Type abilityType))
            {
                Debug.LogError($"Ability type {data.GetType().Name} is not supported.");
                return null;
            }

            var ability = (Ability)_container.Resolve(abilityType);
            ability.SetData(data);
            ability.Start();
            _abilities.Add(ability);

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

        public List<Ability> GetAllUnlocked()
        {
            return _abilities
                .Where(a => YandexGame.savesData.UnlockedAbilityNames
                    .Contains(a.CommonData.AbilityName))
                .ToList();
        }
    }
}