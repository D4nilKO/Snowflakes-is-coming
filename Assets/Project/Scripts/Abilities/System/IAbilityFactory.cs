using System.Collections.Generic;

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