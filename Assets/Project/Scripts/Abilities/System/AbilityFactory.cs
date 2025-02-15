using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Project.Abilities
{
    public class AbilityFactory : IAbilityFactory
    {
        private readonly IObjectResolver _container;

        private HashSet<Ability> _abilities = new();

        public AbilityFactory(IObjectResolver container)
        {
            _container = container;
        }

        public Ability CreateAbility(AbilityData data)
        {
            if (data is SlowModeAbilityData slowModeData)
            {
                var ability = _container.Resolve<SlowModeAbility>();
                ability.SetData(slowModeData);
                _abilities.Add(ability);
                return ability;
            }

            Debug.LogError($"Ability type {data.GetType().Name} is not supported.");
            return null;
        }

        public void Initialize()
        {
            foreach (Ability ability in _abilities)
            {
                ability.Deactivate();
                ability.Reload();
            }
        }

        public IEnumerable<Ability> GetAbilities()
        {
            Debug.LogError(_abilities.Count);
            return _abilities;
        }
    }
}