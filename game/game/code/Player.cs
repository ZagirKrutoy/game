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
        private Texture2D[] runTextures;
        private int currentFrame;
        private double elapsedFrameTime;
        private double frameTime = 100; // Время между кадрами в миллисекундах

        private Vector2 position;
        private float speed;
        private float rotation;
        private Texture2D bulletTexture;
        private List<Bullet> bullets;
        private double lastShootTime;
        private double shootDelay = 200; // Задержка между выстрелами в миллисекундах
        public Vector2 Position => position;
        public List<Bullet> Bullets => bullets;
        public int Health { get; private set; }
        public bool IsAlive => Health > 0;
        private Texture2D healthTexture;
        private bool isMoving;

        public Player()
        {
            position = new Vector2(400, 300); // Начальная позиция персонажа
            speed = 5f;
            bullets = new List<Bullet>();
            Health = 10; // Количество жизней игрока
        }

        public void LoadContent(ContentManager content)
        {
            runTextures = new Texture2D[3];
            runTextures[0] = content.Load<Texture2D>("man_run1");
            runTextures[1] = content.Load<Texture2D>("man");
            runTextures[2] = content.Load<Texture2D>("man_run2");
            bulletTexture = content.Load<Texture2D>("fire2");
            healthTexture = content.Load<Texture2D>("heart");
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            Game1.Instance.playerHitSound.Play();
            if (Health <= 0)
            {
                Health = 0;
                Game1.Instance.playerDeathSound.Play();
                Game1.Instance.PlayerDied();
            }
        }

        public void Update(GameTime gameTime, List<Zombie> zombies)
        {
            KeyboardState state = Keyboard.GetState();
            isMoving = false;

            if (state.IsKeyDown(Keys.A))
            {
                position.X -= speed;
                isMoving = true;
            }
            if (state.IsKeyDown(Keys.D))
            {
                position.X += speed;
                isMoving = true;
            }
            if (state.IsKeyDown(Keys.W))
            {
                position.Y -= speed;
                isMoving = true;
            }
            if (state.IsKeyDown(Keys.S))
            {
                position.Y += speed;
                isMoving = true;
            }

            // Обновление анимации
            if (isMoving)
            {
                elapsedFrameTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (elapsedFrameTime > frameTime)
                {
                    currentFrame = (currentFrame + 1) % runTextures.Length;
                    elapsedFrameTime = 0;
                }
            }
            else
            {
                currentFrame = 1; // Если игрок не движется, показываем кадр "man"
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
            Texture2D currentTexture = runTextures[currentFrame];
            Vector2 origin = new Vector2(currentTexture.Width / 2, currentTexture.Height / 2);
            spriteBatch.Draw(currentTexture, position, null, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 0f);

            foreach (var bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }

            for (int i = 0; i < Health; i++)
            {
                spriteBatch.Draw(healthTexture, new Vector2(10 + i * 80, 10), Color.White);
            }
        }
    }
}
