namespace Project.Abilities
{
    public interface IAbilityFactory
    {
        Ability CreateAbility(AbilityData data);
    }
}