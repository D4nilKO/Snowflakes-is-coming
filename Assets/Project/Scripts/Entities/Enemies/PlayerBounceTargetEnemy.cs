namespace Project.Entities.Enemies
{
    public class PlayerBounceTargetEnemy : StandardEnemy
    {
        protected override void ReflectDirection(bool isOutOfBoundsX, bool isOutOfBoundsY)
        {
            if (isOutOfBoundsX || isOutOfBoundsY)
            {
                TurnInPlayerDirection();
            }
        }
    }
}