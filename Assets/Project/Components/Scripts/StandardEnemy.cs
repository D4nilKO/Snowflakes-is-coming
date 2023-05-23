using UnityEngine;

namespace Project.Components.Scripts
{
    public class StandardEnemy : EnemyBase
    {
        private float objectHeight;
        private float objectWidth;

        protected override void Start()
        {
            base.Start();
            Direction = GetRandomDirection();
        }

        public override void Move()
        {
            Vector2 currentPosition = Rb2D.position;
            Vector2 newPosition = currentPosition + Direction * (speed * Time.fixedDeltaTime);
            var bounds = ObjectCollider.bounds;
            objectWidth = bounds.size.x;
            objectHeight = bounds.size.y;

            if (newPosition.x < -ScreenWidth / 2f + objectWidth / 2f ||
                newPosition.x > ScreenWidth / 2f - objectWidth / 2f)
            {
                ReflectHorizontal(ref newPosition);
            }

            if (newPosition.y < -ScreenHeight / 2f + objectHeight / 2f ||
                newPosition.y > ScreenHeight / 2f - objectHeight / 2f)
            {
                ReflectVertical(ref newPosition);
            }

            Rb2D.MovePosition(newPosition);
        }

        private void ReflectHorizontal(ref Vector2 position)
        {
            Direction = new Vector2(-Direction.x, Direction.y);
            var bounds = ObjectCollider.bounds;
            position.x = Mathf.Clamp(position.x, -ScreenWidth / 2f + bounds.extents.x,
                ScreenWidth / 2f - bounds.extents.x);
        }

        private void ReflectVertical(ref Vector2 position)
        {
            Direction = new Vector2(Direction.x, -Direction.y);
            var bounds = ObjectCollider.bounds;
            position.y = Mathf.Clamp(position.y, -ScreenHeight / 2f + bounds.extents.y,
                ScreenHeight / 2f - bounds.extents.y);
        }
    }
}