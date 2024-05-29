using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace game
{
    public class MainMenu
    {
        public static Texture2D Background { get; set; }
        static int timeCounter = 0;
        static Color color;
        public static SpriteFont Font { get; set; }
        static Vector2 textPosition = new Vector2(760, 200);
        public static Texture2D menuCursorTexture;
        private static Vector2 cursorPosition;

        private List<MenuButton> buttons;
        private SoundEffect menuMusic;
        private SoundEffectInstance menuMusicInstance;
        private VolumeSlider volumeSlider;

        public MainMenu()
        {
            buttons = new List<MenuButton>();
            volumeSlider = new VolumeSlider(new Vector2(760, 850));
        }

        public void LoadContent(ContentManager content)
        {
            Texture2D newGameTexture = content.Load<Texture2D>("new_game");
            Texture2D continueTexture = content.Load<Texture2D>("continue");
            Texture2D exitTexture = content.Load<Texture2D>("exit");

            menuMusic = content.Load<SoundEffect>("menu_music");
            menuMusicInstance = menuMusic.CreateInstance();
            menuMusicInstance.IsLooped = true;
            menuMusicInstance.Play();

            MenuButton newGameButton = new MenuButton(newGameTexture, new Vector2(760, 400));
            newGameButton.OnClick = () =>
            {
                Game1.Instance.StartNewGame();
                menuMusicInstance.Stop();
            };

            MenuButton continueButton = new MenuButton(continueTexture, new Vector2(760, 550));
            continueButton.OnClick = () =>
            {
                Game1.Instance.ContinueGame();
                menuMusicInstance.Stop();
            };

            MenuButton exitButton = new MenuButton(exitTexture, new Vector2(760, 700));
            exitButton.OnClick = () =>
            {
                Game1.Instance.Exit();
            };

            buttons.Add(newGameButton);
            buttons.Add(continueButton);
            buttons.Add(exitButton);

            volumeSlider.LoadContent(content);
        }

        public void Update()
        {
            MouseState mouseState = Mouse.GetState();
            cursorPosition = new Vector2(mouseState.X -5, mouseState.Y - 15);
            color = Color.FromNonPremultiplied(255, 255, 255, timeCounter % 256);
            timeCounter++;

            foreach (var button in buttons)
            {
                button.Update();
            }

            volumeSlider.Update();
            Game1.Instance.SetVolume(volumeSlider.Volume);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Background, Vector2.Zero, Color.White);
            spriteBatch.DrawString(Font, "Zombie Shooter!", textPosition, Color.Red);
            

            foreach (var button in buttons)
            {
                button.Draw(spriteBatch);
            }

            volumeSlider.Draw(spriteBatch);
            spriteBatch.Draw(menuCursorTexture, cursorPosition, Color.White);
        }

        public void PlayMusic()
        {
            if (menuMusicInstance.State != SoundState.Playing)
            {
                menuMusicInstance.Play();
            }
        }

        public void StopMusic()
        {
            if (menuMusicInstance.State == SoundState.Playing)
            {
                menuMusicInstance.Stop();
            }
        }
    }
}
