using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace game
{
    public class MainMenu
    {
        public static Texture2D Background { get; set; }
        static int timeCounter = 0;
        static Color color;
        public static SpriteFont Font { get; set; }
        public static Texture2D menuCursorTexture;
        private static Vector2 cursorPosition;

        private List<MenuButton> buttons;
        private SoundEffect menuMusic;
        private SoundEffectInstance menuMusicInstance;
        private VolumeSlider volumeSlider;

        private Texture2D instructionTexture;
        private bool showInstructions;
        private MenuButton instructionButton;

        public MainMenu()
        {
            buttons = new List<MenuButton>();
            volumeSlider = new VolumeSlider(new Vector2(1385, 1015));
        }

        public void LoadContent(ContentManager content)
        {
            Texture2D newGameTexture = content.Load<Texture2D>("newGameBtn");
            Texture2D newGameHoverTexture = content.Load<Texture2D>("newGameBtnHover");
            Texture2D continueTexture = content.Load<Texture2D>("continueBtn");
            Texture2D continueHoverTexture = content.Load<Texture2D>("continueBtnHover");
            Texture2D exitTexture = content.Load<Texture2D>("exitBtn");
            Texture2D exitHoverTexture = content.Load<Texture2D>("exitBtnHover");
            Texture2D instructionButtonTexture = content.Load<Texture2D>("instructionBtn");
            Texture2D instructionHoverTexture = content.Load<Texture2D>("instructionBtnHover");
            instructionTexture = content.Load<Texture2D>("instructions");

            menuMusic = content.Load<SoundEffect>("menu_music");
            menuMusicInstance = menuMusic.CreateInstance();
            menuMusicInstance.IsLooped = true;
            menuMusicInstance.Play();

            MenuButton newGameButton = new MenuButton(newGameTexture, newGameHoverTexture, new Vector2(160, 300));
            newGameButton.OnClick = () =>
            {
                Game1.Instance.StartNewGame();
                menuMusicInstance.Stop();
            };

            MenuButton continueButton = new MenuButton(continueTexture, continueHoverTexture, new Vector2(160, 460));
            continueButton.OnClick = () =>
            {
                Game1.Instance.ContinueGame();
                menuMusicInstance.Stop();
            };

            MenuButton exitButton = new MenuButton(exitTexture, exitHoverTexture, new Vector2(160, 620));
            exitButton.OnClick = () =>
            {
                Game1.Instance.Exit();
            };

            instructionButton = new MenuButton(instructionButtonTexture, instructionHoverTexture, new Vector2(1800, 30));
            instructionButton.OnClick = () =>
            {
                showInstructions = !showInstructions;
            };

            buttons.Add(newGameButton);
            buttons.Add(continueButton);
            buttons.Add(exitButton);
            buttons.Add(instructionButton);

            volumeSlider.LoadContent(content);
        }

        public void Update()
        {
            MouseState mouseState = Mouse.GetState();
            cursorPosition = new Vector2(mouseState.X - 20, mouseState.Y - 15);

            foreach (var button in buttons)
            {
                button.Update();
            }

            volumeSlider.Update();
            Game1.Instance.SetVolume(volumeSlider.Volume);

            timeCounter++;
            int fadeValue = (int)(Math.Sin(timeCounter * 0.1) * 127 + 128);
            color = new Color(255, 255, 255, fadeValue);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Background, new Vector2(0, 0), Color.White);

            foreach (var button in buttons)
            {
                button.Draw(spriteBatch);
            }

            volumeSlider.Draw(spriteBatch);

            string highScoreText = "High Score: " + Game1.Instance.HighScore;
            spriteBatch.DrawString(Font, highScoreText, new Vector2(1200, 30), Color.DarkRed);

            if (showInstructions)
            {
                Vector2 instructionPosition = new Vector2(
                    (1920 - instructionTexture.Width) / 2,  
                    (1080 - instructionTexture.Height) / 2
                );
                spriteBatch.Draw(instructionTexture, instructionPosition, Color.White);
            }

            spriteBatch.Draw(menuCursorTexture, cursorPosition, color);
        }
    }
}
