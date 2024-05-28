using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace game
{
    public class MenuButton
    {
        private Texture2D texture;
        private Vector2 position;
        private Rectangle bounds;
        public Action OnClick { get; set; }
        private bool isHovered;
        private bool isPressed;

        public MenuButton(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
            bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public void Update()
        {
            MouseState mouseState = Mouse.GetState();
            isHovered = bounds.Contains(mouseState.Position);
            if (isHovered && mouseState.LeftButton == ButtonState.Pressed)
            {
                if (!isPressed)
                {
                    OnClick?.Invoke();
                    isPressed = true;
                }
            }
            else if (mouseState.LeftButton == ButtonState.Released)
            {
                isPressed = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
