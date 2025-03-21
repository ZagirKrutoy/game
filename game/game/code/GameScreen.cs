﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace game
{
    public class GameScreen
    {
        private Texture2D background;
        private Player player;
        private Texture2D[] zombieRunTextures;
        private List<Zombie> zombies;
        private double lastSpawnTime;
        private double spawnDelay = 2000; // Задержка между спавном зомби в миллисекундах
        private SpriteFont gameOverFont;
        public int difficultyLevel = 1;
        private Texture2D cursorTexture;
        private Vector2 cursorPosition;
        public bool PlayerIsAlive => player.IsAlive;
        public int Score { get; private set; }
        private List<Pickup> pickups;
        private Texture2D grenadePickupTexture;
        private Texture2D healthPickupTexture;
        private double lastPickupTime;
        private double pickupInterval = 8000; // Интервал появления пикапов в миллисекундах
        private Vector2[] pickupPositions = new Vector2[]
        {
            new Vector2(200, 200),
            new Vector2(600, 600),
            new Vector2(1400, 800)
        };



        public GameScreen()
        {
            player = new Player();
            zombies = new List<Zombie>();
            pickups = new List<Pickup>();
        }

        public void LoadContent(ContentManager content)
        {
            background = content.Load<Texture2D>("grassBackground");
            player.LoadContent(content);

            // Загрузка текстур анимации зомби
            zombieRunTextures = new Texture2D[3];
            zombieRunTextures[0] = content.Load<Texture2D>("zombie_run1");
            zombieRunTextures[1] = content.Load<Texture2D>("zombie");
            zombieRunTextures[2] = content.Load<Texture2D>("zombie_run2");
            cursorTexture = content.Load<Texture2D>("gameCursorTexture");

            grenadePickupTexture = content.Load<Texture2D>("dropGrenates");
            healthPickupTexture = content.Load<Texture2D>("dropHeart");

            gameOverFont = content.Load<SpriteFont>("GameOverFont"); // Загрузите шрифт для надписи "Игра окончена"
        }

        public void Update(GameTime gameTime)
        {

            MouseState mouseState = Mouse.GetState();
            cursorPosition = new Vector2(mouseState.X -20, mouseState.Y -15);
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
                if (gameTime.TotalGameTime.TotalMilliseconds - lastPickupTime > pickupInterval)
                {
                    CreatePickup();
                    lastPickupTime = gameTime.TotalGameTime.TotalMilliseconds;
                }

                // Обновление пикапов
                foreach (var pickup in pickups)
                {
                    pickup.Update(gameTime, player);
                }

                // Удаление неактивных пикапов
                pickups.RemoveAll(p => !p.IsActive);

            }
        }
        private void CreatePickup()
        {
            Random random = new Random();
            int positionIndex = random.Next(pickupPositions.Length);
            PickupType type = (PickupType)random.Next(2); // 0 или 1 (Grenade или Health)
            Texture2D texture = type == PickupType.Grenade ? grenadePickupTexture : healthPickupTexture;

            Pickup pickup = new Pickup(texture, pickupPositions[positionIndex], type);
            pickups.Add(pickup);
        }
        public void IncreaseDifficulty()
        {
            spawnDelay = Math.Max(500, spawnDelay - 200); // Минимальная задержка между спавном - 500 миллисекунд
            foreach (var zombie in zombies)
            {
                zombie.IncreaseSpeed(0.5f); // Увеличение скорости зомби
            }
        }

        private void SpawnZombie()
        {
            Random random = new Random();
            Vector2 position = new Vector2(random.Next(1920), random.Next(1080));
            Zombie zombie = new Zombie(zombieRunTextures, position, player, 2f + (difficultyLevel - 1) * 0.5f); // Скорость увеличивается с каждым уровнем сложности
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
                spriteBatch.DrawString(gameOverFont, gameOverText, position, Color.DarkRed);

                string finalScoreText = "Score: " + Game1.Instance.LastScore;
                Vector2 finalScoreTextSize = gameOverFont.MeasureString(finalScoreText);
                Vector2 finalScorePosition = new Vector2(1920 / 2 - finalScoreTextSize.X / 2, 1080 / 2 - finalScoreTextSize.Y / 2 + 70);
                spriteBatch.DrawString(gameOverFont, finalScoreText, finalScorePosition, Color.DarkRed);
            }
            foreach (var pickup in pickups)
            {
                pickup.Draw(spriteBatch);
            }
            spriteBatch.Draw(cursorTexture, cursorPosition, Color.White);
        }
    }
}
