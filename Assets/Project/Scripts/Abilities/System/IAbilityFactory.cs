using System.Collections.Generic;
using UnityEngine;

namespace Project.Abilities
{
    public interface IAbilityFactory
    {
        Ability CreateAbility(AbilityData data);

        List<Ability> GetAllAbilities();
        List<Ability> GetUnlockedAbilities();
        void Initialize();
    }
}