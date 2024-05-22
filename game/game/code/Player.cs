using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace game
{
    public class Player
    {
        private Texture2D texture;
        private Vector2 position;
        private float speed;
        private float rotation;
        private Texture2D bulletTexture;
        private List<Bullet> bullets;
        private double lastShootTime;
        private double shootDelay = 200; // Задержка между выстрелами в миллисекундах
        public Vector2 Position => position;
        public List<Bullet> Bullets => bullets;



        public Player()
        {
            position = new Vector2(400, 300); // Начальная позиция персонажа
            speed = 5f; 
            bullets = new List<Bullet>();
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("man"); 
            bulletTexture = content.Load<Texture2D>("fire2");
        }

        public void Update(GameTime gameTime, List<Zombie> zombies)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.A))
            {
                position.X -= speed;
            }
            if (state.IsKeyDown(Keys.D))
            {
                position.X += speed;
            }
            if (state.IsKeyDown(Keys.W))
            {
                position.Y -= speed;
            }
            if (state.IsKeyDown(Keys.S))
            {
                position.Y += speed;
            }

            // Обновление угла поворота в сторону курсора
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            Vector2 direction = mousePosition - position;
            rotation = (float)Math.Atan2(direction.Y, direction.X);

            if (mouseState.LeftButton == ButtonState.Pressed && (gameTime.TotalGameTime.TotalMilliseconds - lastShootTime) > shootDelay)
                Shoot(gameTime.TotalGameTime.TotalMilliseconds);

            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update(gameTime, zombies);
                if (!bullets[i].IsActive)
                    bullets.RemoveAt(i);
            }
        }

        private void Shoot(double currentTime)
        {
            Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            Bullet bullet = new Bullet(bulletTexture, position, direction);
            bullets.Add(bullet);
            lastShootTime = currentTime; 
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2); // Центр спрайта
            spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 0f);
            foreach (var bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }
        }
    }
}
