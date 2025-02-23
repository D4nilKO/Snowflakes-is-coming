namespace Project.Abilities
{
    public interface IHasCooldown
    {
        float GetCooldown();
        float GetRemainingCooldown();
        bool IsReady();
    }
}