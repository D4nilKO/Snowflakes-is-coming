namespace Project.Entities.Enemies
{
    public class PlayerDirectedEnemy : StandardEnemy
    {
        protected override void SetDirectionOnSpawn()
        {
            TurnInPlayerDirection();
        }
    }
}