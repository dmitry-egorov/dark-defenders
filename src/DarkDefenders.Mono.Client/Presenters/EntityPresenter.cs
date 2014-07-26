using Infrastructure.Math;
using Infrastructure.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkDefenders.Mono.Client.Presenters
{
    internal class EntityPresenter
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly float _width;
        private readonly float _height;
        private readonly Texture2D _texture;
        private readonly Color _color;

        private Vector2 _position;
        private Direction _direction;

        public EntityPresenter(SpriteBatch spriteBatch, Vector initialPosition, EntityProperties properties)
        {
            _position = Transform(initialPosition);
            _spriteBatch = spriteBatch;
            _width = properties.Width;
            _height = properties.Height;
            _texture = properties.Texture;
            _color = properties.Color;
        }

        public void SetPosition(Vector value)
        {
            _position = Transform(value);
        }

        public void SetDirection(Direction newDirection)
        {
            _direction = newDirection;
        }

        public void Draw()
        {
            var scale = new Vector2(_width / _texture.Width, _height / _texture.Height);

            var spriteEffects = SpriteEffects.FlipVertically;

            if (_direction == Direction.Left)
            {
                spriteEffects |= SpriteEffects.FlipHorizontally;
            }

            _spriteBatch.Draw(_texture, _position, null, _color, 0, Vector2.Zero, scale, spriteEffects, 0);
        }

        private Vector2 Transform(Vector position)
        {
            return new Vector2(position.X.ToSingle() - (_width / 2), position.Y.ToSingle() - (_height / 2));
        }
    }
}