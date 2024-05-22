using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

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
        Texture2D background;
        Stat Stat = Stat.SplashScreen;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            SplashScreen.Background = Content.Load<Texture2D>("background");
            SplashScreen.Font = Content.Load<SpriteFont>("SplashFont");

            gameScreen = new GameScreen();
            gameScreen.LoadContent(Content); 
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {       
            KeyboardState keyboardState = Keyboard.GetState();
            switch (Stat)
            {
                case Stat.SplashScreen:
                    SplashScreen.Update();
                    //if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape)))
                      //  Exit();
                    if (keyboardState.IsKeyDown(Keys.Space)) Stat = Stat.Game;
                    break;
                case Stat.Game:
                    gameScreen.Update(gameTime);
                    if (keyboardState.IsKeyDown(Keys.Escape)) Stat = Stat.SplashScreen;
                    
                    break;
            }
            base.Update(gameTime);
            

            // TODO: Add your update logic here
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            switch(Stat)
            {
                case Stat.SplashScreen:
                    SplashScreen.Draw(_spriteBatch);
                    break;
                case Stat.Game:
                    gameScreen.Draw(_spriteBatch);
                    break;
            }         
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}




