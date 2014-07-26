using Infrastructure.Math;
using Infrastructure.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkDefenders.Mono.Client.Presenters
{
    internal class EntityPresenter
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _whiteTexture;
        private readonly Color _color;

        private Vector2 _position;

        public void SetPosition(Vector value)
        {
            _position = Transform(value);
        }

        public EntityPresenter(Vector initialPosition, SpriteBatch spriteBatch, Texture2D whiteTexture, Color color)
        {
            _position = Transform(initialPosition);
            _spriteBatch = spriteBatch;
            _whiteTexture = whiteTexture;
            _color = color;
        }

        public void Draw()
        {
            _spriteBatch.Draw(_whiteTexture, _position, new Rectangle(0, 0, 1, 1), _color);
        }

        private static Vector2 Transform(Vector position)
        {
            return new Vector2(position.X.ToSingle() - 0.5f, position.Y.ToSingle() - 0.5f);
        }
    }
}