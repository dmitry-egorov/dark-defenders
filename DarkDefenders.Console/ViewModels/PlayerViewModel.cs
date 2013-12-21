using System.Drawing;
using DarkDefenders.Domain.RigidBodies.Events;
using Infrastructure.Math;

namespace DarkDefenders.Console.ViewModels
{
    internal class PlayerViewModel
    {
        private readonly int _width;
        private readonly int _height;

        private Point _lastPlayerPosition;

        public PlayerViewModel(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void Apply(RigidBodyCreated rigidBodyCreated)
        {
            var position = Transform(rigidBodyCreated.BoundingCircle.Position);

            ConsoleRenderer.Render(position, '@');

            _lastPlayerPosition = position;
        }

        public void Apply(Moved moved)
        {
            var position = Transform(moved.NewPosition);

            if (position == _lastPlayerPosition)
            {
                return;
            }

            ConsoleRenderer.Render(_lastPlayerPosition, ' ');
            ConsoleRenderer.Render(position, '@');

            _lastPlayerPosition = position;
        }

        private Point Transform(Vector spawnPosition)
        {
            var x = spawnPosition.X;
            var y = spawnPosition.Y;
            var cx = 1 + (int)((_width - 2) * (1 + x) / 2);
            var cy = 1 + (int)((_height - 2) * (1 - y));

            return new Point(cx, cy);
        }
    }
}