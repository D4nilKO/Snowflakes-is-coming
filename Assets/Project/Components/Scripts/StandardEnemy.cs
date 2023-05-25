using UnityEngine;

namespace Project.Components.Scripts
{
    public class StandardEnemy : EnemyBase
    {
        protected override void Start()
        {
            base.Start();
            Direction = GetRandomDirection();
        }

        public override void Move()
        {
            Vector2 currentPosition = Rb2D.position;
            Vector2 newPosition = currentPosition + Direction * (speed * Time.fixedDeltaTime);

            CheckOutOfBounds(ref newPosition);
            
            Rb2D.MovePosition(newPosition);
        }

        private void CheckOutOfBounds(ref Vector2 _newPosition)
        {
            if (_newPosition.x < -ScreenWidth / 2f + ObjectWidth / 2f ||
                _newPosition.x > ScreenWidth / 2f - ObjectWidth / 2f)
            {
                ReflectHorizontal(ref _newPosition);
            }

            if (_newPosition.y < -ScreenHeight / 2f + ObjectHeight / 2f ||
                _newPosition.y > ScreenHeight / 2f - ObjectHeight / 2f)
            {
                ReflectVertical(ref _newPosition);
            }
        }

        
    }
}