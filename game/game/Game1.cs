using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace game
{
    enum Stat
    {
        SplashScreen,
        Game,
        Final,
        Pause
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameScreen gameScreen;
        private MainMenu mainMenu;
        public static Game1 Instance { get; private set; }
        Texture2D background;
        Stat Stat = Stat.SplashScreen;

        private SoundEffect menuMusic;
        private SoundEffectInstance menuMusicInstance;
        private SoundEffect gameMusic;
        private SoundEffectInstance gameMusicInstance;

        public SoundEffect zombieHitSound;
        public SoundEffect playerHitSound;
        public SoundEffect playerDeathSound;
        private int score;
        private SpriteFont scoreFont;
        private int lastScore;
        




        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Instance = this;
            score = 0;
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            MainMenu.Background = Content.Load<Texture2D>("grassBackground");
            MainMenu.Font = Content.Load<SpriteFont>("SplashFont");
            MainMenu.menuCursorTexture = Content.Load<Texture2D>("menuCursorTexture");

            gameScreen = new GameScreen();
            gameScreen.LoadContent(Content);

            mainMenu = new MainMenu();
            mainMenu.LoadContent(Content);

            menuMusic = Content.Load<SoundEffect>("menu_music");
            menuMusicInstance = menuMusic.CreateInstance();
            menuMusicInstance.IsLooped = true;
            menuMusicInstance.Play();

            gameMusic = Content.Load<SoundEffect>("game_music");
            gameMusicInstance = gameMusic.CreateInstance();
            gameMusicInstance.IsLooped = true;

            zombieHitSound = Content.Load<SoundEffect>("zombieDamageSound");
            playerHitSound = Content.Load<SoundEffect>("playerDamageSound");
            playerDeathSound = Content.Load<SoundEffect>("dead_sound");
            scoreFont = Content.Load<SpriteFont>("ScoreFont");
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            
            switch (Stat)
            {
                case Stat.SplashScreen:
                    mainMenu.Update();
                    if (keyboardState.IsKeyDown(Keys.Space))
                    {
                        Stat = Stat.Game;
                        menuMusicInstance.Stop();
                        gameMusicInstance.Play();
                    }
                    break;
                case Stat.Game:
                    gameScreen.Update(gameTime);
                    if (keyboardState.IsKeyDown(Keys.Escape))
                    {
                        Stat = Stat.SplashScreen;
                        gameMusicInstance.Stop();
                        menuMusicInstance.Play();
                    }
                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            switch (Stat)
            {
                case Stat.SplashScreen:
                    mainMenu.Draw(_spriteBatch);
                    break;
                case Stat.Game:
                    gameScreen.Draw(_spriteBatch);
                    DrawScore();
                    break;
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public void StartNewGame()
        {
            Stat = Stat.Game;
            gameScreen = new GameScreen();
            gameScreen.LoadContent(Content);
            menuMusicInstance.Stop();
            gameMusicInstance.Play();
            ResetScore();
        }

        public void ContinueGame()
        {
            Stat = Stat.Game;
            menuMusicInstance.Stop();
            gameMusicInstance.Play();
        }

        public void PlayerDied()
        {
            gameMusicInstance.Stop();
            lastScore = score;
            ResetScore();
        }

        public void SetVolume(float volume)
        {

            SoundEffect.MasterVolume = volume;
        }
        public void IncreaseScore(int amount)
        {
            score += amount;
            if (score / 100 >= gameScreen.difficultyLevel)
            {
                gameScreen.difficultyLevel++;
                gameScreen.IncreaseDifficulty();
            }
        }
        public void ResetScore()
        {
            score = 0;
        }


        private void DrawScore()
        {
            if (gameScreen.PlayerIsAlive)
            {
                string scoreText = "Score: " + score;
                _spriteBatch.DrawString(scoreFont, scoreText, new Vector2(1600, 20), Color.White);
            }
        }
        public int LastScore => lastScore;
    }
}
