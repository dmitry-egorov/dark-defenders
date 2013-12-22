using System;
using System.Drawing;
using DarkDefenders.Domain.Players.Events;
using DarkDefenders.Domain.RigidBodies.Events;
using Infrastructure.Math;

namespace DarkDefenders.Console.ViewModels
{
    internal class RigidBodyViewModel
    {
        private readonly int _width;
        private readonly int _height;

        private Point _lastPlayerPosition;
        private char? _character;

        public RigidBodyViewModel(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void SetAsPlayer()
        {
            _character = '@';
            ConsoleRenderer.Render(_lastPlayerPosition, _character.Value);
        }

        public void SetAsProjectile()
        {
            _character = '*';
            ConsoleRenderer.Render(_lastPlayerPosition, _character.Value);
        }

        public void Apply(RigidBodyCreated rigidBodyCreated)
        {
            _lastPlayerPosition = Transform(rigidBodyCreated.BoundingCircle.Position);
        }

        public void Apply(Moved moved)
        {
            if (!_character.HasValue)
            {
                throw new InvalidOperationException("Rigid body type not set");
            }

            var position = Transform(moved.NewPosition);

            if (position == _lastPlayerPosition)
            {
                return;
            }

            ConsoleRenderer.Render(_lastPlayerPosition, ' ');
            ConsoleRenderer.Render(position, _character.Value);

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