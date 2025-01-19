using Project.Abilities;
using UnityEngine;
using VContainer;

public class AbilityFactory : IAbilityFactory
{
    private readonly IObjectResolver _container;

    public AbilityFactory(IObjectResolver container)
    {
        _container = container;
    }

    public Ability CreateAbility(AbilityData data)
    {
        if (data is SlowModeAbilityData slowModeData)
        {
            // Создаём способность через контейнер
            var ability = _container.Resolve<SlowModeAbility>();
            ability.SetData(slowModeData);
            return ability;
        }

        // Обработка других типов способностей
        Debug.LogError($"Ability type {data.GetType()} is not supported.");
        return null;
    }
}