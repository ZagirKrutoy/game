using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace game
{
    public class Bullet
    {
        private Texture2D texture;
        private Vector2 position;
        private Vector2 direction;
        private float speed;

        public bool IsActive { get; private set; }

        public Bullet(Texture2D texture, Vector2 position, Vector2 direction)
        {
            this.texture = texture;
            this.position = position;
            this.direction = direction;
            speed = 16f;
            IsActive = true;
        }

        public void Update(GameTime gameTime, List<Zombie> zombies)
        {
            position += direction * speed;
            if (position.X < 0 || position.X > 1920 || position.Y < 0 || position.Y > 1080)
            {
                IsActive = false;
            }

            // Проверка столкновения пули с зомби
            foreach (var zombie in zombies)
            {
                if (GetHitbox().Intersects(zombie.GetHitbox()))
                {
                    IsActive = false; // Пуля исчезает при попадании
                    zombie.TakeDamage(); // Зомби получает урон
                }
            }
        }

        public Rectangle GetHitbox()
        {
            // Возвращает хитбокс пули
            return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
