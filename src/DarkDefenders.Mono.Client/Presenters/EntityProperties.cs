using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkDefenders.Mono.Client.Presenters
{
    public class EntityProperties
    {
        public float Width { get; private set; }
        public float Height { get; private set; }
        public Texture2D Texture { get; private set; }
        public Color Color { get; private set; }

        public EntityProperties(float width, float height, Texture2D texture, Color color)
        {
            Width = width;
            Height = height;
            Texture = texture;
            Color = color;
        }
    }
}