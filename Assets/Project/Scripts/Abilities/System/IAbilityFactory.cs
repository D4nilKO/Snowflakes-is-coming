using System.Collections.Generic;

namespace Project.Abilities
{
    public interface IAbilityFactory
    {
        Ability CreateAbility(AbilityData data);

        IEnumerable<Ability> GetAbilities();
        void Initialize();
    }
}