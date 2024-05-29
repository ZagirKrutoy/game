using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace game
{
    public class Zombie
    {
        private Texture2D[] runTextures;
        private int currentFrame;
        private double frameTime;
        private double timeSinceLastFrame;
        private Vector2 position;
        private float speed;
        private Player player;
        private int health = 2;
        private float rotation;
        public double lastAttackTime;
        private const double attackDelay = 1000; // Задержка между атаками в миллисекундах
        private int damage = 1; // Урон, наносимый зомби
        private float hitboxOffset = 20f; // Смещение хитбокса в сторону левого плеча
        private float hitboxRadius = 15f; // Радиус хитбокса

        public bool IsActive { get; private set; }

        public Zombie(Texture2D[] runTextures, Vector2 startPosition, Player player, float speed)
        {
            this.runTextures = runTextures;
            this.position = startPosition;
            this.player = player;
            this.speed = speed; // Скорость зомби
            IsActive = true;
            lastAttackTime = 0;
            currentFrame = 0;
            frameTime = 200; // Время между кадрами в миллисекундах
            timeSinceLastFrame = 0;
        }

        public void Update(GameTime gameTime, List<Zombie> allZombies)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timeSinceLastFrame >= frameTime)
            {
                currentFrame = (currentFrame + 1) % runTextures.Length;
                timeSinceLastFrame = 0;
            }

            Vector2 direction = player.Position - position;
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }
            rotation = (float)Math.Atan2(direction.Y, direction.X);
            Vector2 targetPosition = position + direction * speed;
            if (!IsPositionOccupied(targetPosition, allZombies))
            {
                position = targetPosition;
            }
            if (Vector2.Distance(position, player.Position) < 50) // Радиус игрока и зомби 50
            {
                if (gameTime.TotalGameTime.TotalMilliseconds - lastAttackTime > attackDelay)
                {
                    player.TakeDamage(damage);
                    lastAttackTime = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }

            // Зомби активен, пока он в пределах экрана
            if (position.X < 0 || position.X > 1920 || position.Y < 0 || position.Y > 1080)
            {
                IsActive = false;
            }
        }

        bool IsPositionOccupied(Vector2 positionToCheck, List<Zombie> allZombies)
        {
            foreach (var otherZombie in allZombies)
            {
                if (otherZombie != this && Vector2.Distance(positionToCheck, otherZombie.position) < hitboxRadius)
                {
                    return true; // Позиция занята другим зомби
                }
            }
            return false; // Позиция свободна
        }

        public void TakeDamage()
        {
            health--;
            Game1.Instance.zombieHitSound.Play();
            if (health <= 0)
            {
                IsActive = false;
                Game1.Instance.IncreaseScore(10); // Увеличение очков на 10 за убитого зомби
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(runTextures[currentFrame].Width / 2, runTextures[currentFrame].Height / 2);
            spriteBatch.Draw(runTextures[currentFrame], position, null, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 0f);
        }

        public Rectangle GetHitbox()
        {
            Vector2 origin = new Vector2(runTextures[currentFrame].Width / 2, runTextures[currentFrame].Height / 2);
            Vector2 hitboxPosition = position + new Vector2((float)Math.Cos(rotation) * hitboxOffset, (float)Math.Sin(rotation) * hitboxOffset);
            return new Rectangle((int)(hitboxPosition.X - hitboxRadius), (int)(hitboxPosition.Y - hitboxRadius), (int)(hitboxRadius * 2), (int)(hitboxRadius * 2));
        }

        public void IncreaseSpeed(float amount)
        {
            speed += amount;
        }
    }
}
