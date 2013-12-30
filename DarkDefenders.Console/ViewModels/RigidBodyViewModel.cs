using System;
using System.Drawing;
using DarkDefenders.Domain.RigidBodies.Events;
using Infrastructure.Math;

namespace DarkDefenders.Console.ViewModels
{
    internal class RigidBodyViewModel
    {
        private Point _transformedLastPosition;
        private char? _character;
        private readonly Map _map;
        private Point _lastPosition;

        public RigidBodyViewModel(Map map)
        {
            _map = map;
        }

        public void SetAsPlayer()
        {
            _character = '@';
            ConsoleRenderer.Render(_transformedLastPosition, _character.Value);
        }

        public void SetAsProjectile()
        {
            _character = '*';
            ConsoleRenderer.Render(_transformedLastPosition, _character.Value);
        }

        public void Recieve(RigidBodyCreated rigidBodyCreated)
        {
            var position = rigidBodyCreated.BoundingCircle.Position;
            var point = position.ToPoint();
            _lastPosition = point;
            _transformedLastPosition = Transform(point);
        }

        public void Recieve(Moved moved)
        {
            if (!_character.HasValue)
            {
                throw new InvalidOperationException("Rigid body type not set");
            }

            var newPosition = moved.NewPosition.ToPoint();
            var position = Transform(newPosition);

            if (position == _transformedLastPosition)
            {
                return;
            }

            ConsoleRenderer.Render(position, _character.Value);
            Remove();

            _lastPosition = newPosition;
            _transformedLastPosition = position;
        }

        public void Remove()
        {
            var c = _map[_lastPosition] == 1 ? '+' : ' ';

            ConsoleRenderer.Render(_transformedLastPosition, c);
        }

        private Point Transform(Point position)
        {
            var x = position.X;
            var y = position.Y;
            var cx = 1 + x;
            var cy = _map.Dimensions.Height - y;

            return new Point(cx, cy);
        }
    }
}