using Infrastructure.Math;
using Microsoft.Xna.Framework;

namespace DarkDefenders.Mono.Client.Presenters
{
    public class Camera
    {
        private readonly int _screenWidth;
        private readonly int _screenHeight;

        private readonly float _zoom;

        private Vector _position;

        public Camera(int screenWidth, int screenHeight, float initialZoom, Vector initialPosition)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _zoom = initialZoom;
            _position = initialPosition;
        }

        public Matrix GetProjectionMatrix()
        {
            var scale = _zoom * _screenHeight / 768;
            return Matrix.CreateTranslation((float) -_position.X, (float) -_position.Y, 0)
                 * Matrix.CreateScale(scale, -scale, 0)
                 * Matrix.CreateTranslation(_screenWidth / 2.0f, _screenHeight / 2.0f, 0.0f);
        }

        public Vector GetPosition()
        {
            return _position;
        }

        public void SetPosition(Vector newPosition)
        {
            _position = newPosition;
        }
    }
}