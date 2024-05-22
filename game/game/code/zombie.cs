using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace game
{
    public class Zombie
    {
        private Texture2D texture;
        private Vector2 position;
        private float speed;
        private Player player;

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

            position += direction * speed;

            // Зомби активен, пока он в пределах экрана
            if (position.X < 0 || position.X > 1920 || position.Y < 0 || position.Y > 1080)
            {
                IsActive = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
