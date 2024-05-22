using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace game
{
    public class Zombie
    {
        public Texture2D texture;
        public Vector2 position;
        private float speed;
        private Player player;
        public int health = 2;
        private float rotation;

        public bool IsActive { get; private set; }

        public Zombie(Texture2D texture, Vector2 startPosition, Player player)
        {
            this.texture = texture;
            this.position = startPosition;
            this.player = player;
            speed = 2f; // Скорость зомби
            IsActive = true;
        }

        public void Update(GameTime gameTime)
        {
            Vector2 direction = player.Position - position;
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }
            rotation = (float)Math.Atan2(direction.Y, direction.X);
            position += direction * speed;

            // Зомби активен, пока он в пределах экрана
            if (position.X < 0 || position.X > 1920 || position.Y < 0 || position.Y > 1080)
            {
                IsActive = false;
            }
        }   
        public void TakeDamage()
        {
            // Уменьшаем здоровье зомби или устанавливаем IsActive в false, если здоровье меньше или равно нулю
            health --;
            if (health <= 0)
            {
                IsActive = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, rotation, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
        }
    }
}
