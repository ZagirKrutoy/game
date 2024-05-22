using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace game
{
    public class Zombie
    {
        public Texture2D texture;
        public Vector2 position;
        private float speed;
        private Player player;
        private int health = 2;
        private float rotation;
        private double lastAttackTime;
        private const double attackDelay = 2000; // Задержка между атаками в миллисекундах
        private int damage = 1; // Урон, наносимый зомби


        public bool IsActive { get; private set; }

        public Zombie(Texture2D texture, Vector2 startPosition, Player player)
        {
            this.texture = texture;
            this.position = startPosition;
            this.player = player;
            speed = 2f; // Скорость зомби
            IsActive = true;
            lastAttackTime = 0;
        }

        public void Update(GameTime gameTime, List<Zombie> allZombies)
        {
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
                    if (gameTime.TotalGameTime.TotalMilliseconds - lastAttackTime > attackDelay)
                    {
                        player.TakeDamage(damage);
                        lastAttackTime = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
            }

            if (position.X < 0 || position.X > 1920 || position.Y < 0 || position.Y > 1080)
            {
                IsActive = false;
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
                if (otherZombie != this && Vector2.Distance(positionToCheck, otherZombie.position) < 30)
                {
                    return true; // Позиция занята другим зомби
                }
            }
            return false; // Позиция свободна
        }

        public void TakeDamage()
        {
            health--;
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
