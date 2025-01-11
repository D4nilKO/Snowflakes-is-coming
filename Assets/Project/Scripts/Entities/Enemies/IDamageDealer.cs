using Project.GameState;

namespace Project.Entities.Enemies
{
    public interface IDamageDealer
    {
        public void DealDamage(GameOutcome gameOutcome);
    }
}