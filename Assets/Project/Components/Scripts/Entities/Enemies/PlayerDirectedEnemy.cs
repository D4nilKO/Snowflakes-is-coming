using Project.Entities.Character;

namespace Project.Entities.Enemies
{
    public class PlayerDirectedEnemy : StandardEnemy
    {
        public override void OnSpawn()
        {
            TurnInPlayerDirection();
        }
    }
}