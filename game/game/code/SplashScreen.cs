using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace game
{
    internal class SplashScreen
    {
        public static Texture2D Background {  get; set; }
        static int timeCounter = 0;
        static Color color;
        public static SpriteFont Font { get; set; }
        static Vector2 textPosition = new Vector2(760, 200);

        static public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Background, Vector2.Zero, Color.White);    
            spriteBatch.DrawString(Font, "Zombie Shooter!", textPosition, Color.Red);
        }

        static public void Update()
        {
            color = Color.FromNonPremultiplied(255, 255, 255, timeCounter % 256);
            timeCounter++;
        }
    }
}
