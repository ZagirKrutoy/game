using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game
{
    public enum PickupType
    {
        Grenade,
        Health
    }

    public class Pickup
    {
        private Texture2D texture;
        public Vector2 Position { get; private set; }
        public bool IsActive { get; private set; }
        public PickupType Type { get; private set; }

        public Pickup(Texture2D texture, Vector2 position, PickupType type)
        {
            this.texture = texture;
            this.Position = position;
            this.Type = type;
            this.IsActive = true;
        }

        public void Update(GameTime gameTime, Player player)
        {
            if (IsActive && GetHitbox().Intersects(player.GetHitbox()))
            {
                IsActive = false;
                if (Type == PickupType.Grenade)
                {
                    player.IncreaseGrenades();
                }
                else if (Type == PickupType.Health)
                {
                    player.IncreaseHealth();
                }
            }
        }

        public Rectangle GetHitbox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                spriteBatch.Draw(texture, Position, Color.White);
            }
        }
    }
}
