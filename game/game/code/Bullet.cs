using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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

        public void Update(GameTime gameTime)
        {
            position += direction * speed;
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
