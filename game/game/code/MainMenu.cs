using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace game
{
    public class MainMenu
    {
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
            foreach (var button in buttons)
            {
                button.Update();
            }

            volumeSlider.Update();
            Game1.Instance.SetVolume(volumeSlider.Volume);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var button in buttons)
            {
                button.Draw(spriteBatch);
            }

            volumeSlider.Draw(spriteBatch);
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
