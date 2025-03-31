using System.Collections.Generic;
using UnityEngine;

namespace Project.Abilities
{
    public interface IAbilityFactory
    {
        Ability Create(AbilityData data);

        List<Ability> GetAll();
        List<Ability> GetAllUnlocked();

        void Initialize();
    }
}