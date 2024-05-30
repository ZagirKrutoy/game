using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace game
{
    public class Grenade
    {
        private Texture2D texture;
        private Vector2 position;
        private Vector2 direction;
        private float speed = 7f;
        private bool isActive;
        private double explosionTime;
        private double explosionDelay = 1500; // Задержка перед взрывом в миллисекундах
        private float explosionRadius = 200f; // Радиус взрыва гранаты
        private int damage = 5; // Урон от взрыва гранаты

        public bool IsActive => isActive;

        public Grenade(Texture2D texture, Vector2 startPosition, Vector2 direction)
        {
            this.texture = texture;
            this.position = startPosition;
            this.direction = direction;
            isActive = true;
            explosionTime = 0;
        }

        public void Update(GameTime gameTime, List<Zombie> zombies)
        {
            if (isActive)
            {
                position += direction * speed;

                // Проверка столкновения пули с зомби
                foreach (var zombie in zombies)
                {
                    if (GetHitbox().Intersects(zombie.GetHitbox()))
                    {
                        isActive = false; // Пуля исчезает при попадании
                        Explode(zombies); // Зомби получает урон
                    }
                }

                explosionTime += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (explosionTime > explosionDelay)
                {
                    Explode(zombies);
                }

                // Ограничение максимального расстояния
                if (Vector2.Distance(position, Player.position) > 700) // Макс. расстояние 500
                {
                    isActive = false;
                }
            }
        }

        private void Explode(List<Zombie> zombies)
        {
            isActive = false;

            foreach (var zombie in zombies)
            {
                if (Vector2.Distance(position, zombie.position) < explosionRadius)
                {
                    zombie.TakeDamage(damage);
                }
            }

            // Воспроизведение звука взрыва
            Game1.Instance.grenadeExplosionSound.Play();
        }
        public Rectangle GetHitbox()
        {
            // Возвращает хитбокс пули
            return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                spriteBatch.Draw(texture, position, Color.White);
            }
        }
    }
}
