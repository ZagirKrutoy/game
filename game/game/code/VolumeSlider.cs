using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace game
{
    public class VolumeSlider
    {
        private Texture2D sliderBarTexture;
        private Texture2D sliderButtonTexture;
        private Vector2 position;
        private Rectangle sliderRectangle;
        private Rectangle buttonRectangle;
        private bool isDragging;
        private int sliderWidth;

        public float Volume { get; private set; }

        public VolumeSlider(Vector2 position)
        {
            this.position = position;
            this.sliderWidth = 512; // Ширина ползунка
        }

        public void LoadContent(ContentManager content)
        {
            sliderBarTexture = content.Load<Texture2D>("VolumeSlider");
            sliderButtonTexture = content.Load<Texture2D>("VolumeBtn");
            sliderRectangle = new Rectangle((int)position.X, (int)position.Y, sliderWidth, sliderBarTexture.Height);
            buttonRectangle = new Rectangle((int)position.X, (int)position.Y - (sliderButtonTexture.Height / 2) + (sliderRectangle.Height / 2), sliderButtonTexture.Width, sliderButtonTexture.Height);
        }

        public void Update()
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (!isDragging && buttonRectangle.Contains(mouseState.Position))
                {
                    isDragging = true;
                }

                if (isDragging)
                {
                    int newButtonX = Math.Clamp(mouseState.X, sliderRectangle.Left, sliderRectangle.Right - buttonRectangle.Width);
                    buttonRectangle.X = newButtonX;
                    Volume = (float)(newButtonX - sliderRectangle.Left) / (sliderRectangle.Width - buttonRectangle.Width);
                }
            }
            else
            {
                isDragging = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sliderBarTexture, sliderRectangle, Color.White);
            spriteBatch.Draw(sliderButtonTexture, buttonRectangle, Color.White);
        }
    }
}
