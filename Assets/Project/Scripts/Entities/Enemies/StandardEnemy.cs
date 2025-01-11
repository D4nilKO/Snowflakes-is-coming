using Project.GameState;

namespace Project.Entities.Enemies
{
    public class StandardEnemy : EnemyBase, IDamageDealer
    {
        public void DealDamage(GameOutcome gameOutcome)
        {
            gameOutcome.LostGame();
        }
    }
}