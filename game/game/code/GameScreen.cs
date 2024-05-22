using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace game
{
    public class GameScreen
    {
        private Texture2D background;
        private Player player;
        private Texture2D zombieTexture;
        private List<Zombie> zombies;
        private double lastSpawnTime;
        private double spawnDelay = 2000; // Задержка между спавном зомби в миллисекундах
        private SpriteFont gameOverFont;


        public GameScreen()
        {
            player = new Player();
            zombies = new List<Zombie>();
        }

        public void LoadContent(ContentManager content)
        {
            background = content.Load<Texture2D>("grassBackground");
            player.LoadContent(content);
            zombieTexture = content.Load<Texture2D>("zombie");
            gameOverFont = content.Load<SpriteFont>("GameOverFont"); // Загрузите шрифт для надписи "Игра окончена"
        }
        public void Update(GameTime gameTime)
        {
            if (player.IsAlive)
            {
                player.Update(gameTime, zombies);

                if (gameTime.TotalGameTime.TotalMilliseconds - lastSpawnTime > spawnDelay)
                {
                    SpawnZombie();
                    lastSpawnTime = gameTime.TotalGameTime.TotalMilliseconds;
                }

                for (int i = zombies.Count - 1; i >= 0; i--)
                {
                    zombies[i].Update(gameTime, zombies);
                    if (!zombies[i].IsActive)
                    {
                        zombies.RemoveAt(i);
                    }
                }
            }
        }

        private void SpawnZombie()
        {
            // Спавним зомби в случайной позиции
            Random random = new Random();
            Vector2 position = new Vector2(random.Next(1920), random.Next(1080));
            Zombie zombie = new Zombie(zombieTexture, position, player);
            zombies.Add(zombie);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            player.Draw(spriteBatch);

            foreach (var zombie in zombies)
            {
                zombie.Draw(spriteBatch);
            }
            if (!player.IsAlive)
            {
                string gameOverText = "Game over";
                Vector2 textSize = gameOverFont.MeasureString(gameOverText);
                Vector2 position = new Vector2(1920 / 2 - textSize.X / 2, 1080 / 2 - textSize.Y / 2);
                spriteBatch.DrawString(gameOverFont, gameOverText, position, Color.Red);
            }
        }

    }
}
