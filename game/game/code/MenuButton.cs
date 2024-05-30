using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace game
{
    public class MenuButton
    {
        private Texture2D texture;
        private Texture2D hoverTexture;
        private Vector2 position;
        private Rectangle bounds;
        public Action OnClick { get; set; }
        private bool isHovered;
        private bool isPressed;

        public MenuButton(Texture2D texture, Texture2D hoverTexture, Vector2 position)
        {
            this.texture = texture;
            this.hoverTexture = hoverTexture;
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
            if (isHovered)
            {
                spriteBatch.Draw(hoverTexture, position, Color.White);
            }
            else
            {
                spriteBatch.Draw(texture, position, Color.White);
            }
        }
    }
}
