using System;
using System.Drawing;
using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.RigidBodies.Events;
using Infrastructure.Math;

namespace DarkDefenders.Console.ViewModels
{
    internal class RigidBodyViewModel
    {
        public RigidBodyViewModel(Map<Tile> map, ConsoleRenderer consoleRenderer)
        {
            _map = map;
            _consoleRenderer = consoleRenderer;
        }

        public void SetAsPlayersAvatar()
        {
            SetType('@', ConsoleColor.Cyan);
        }

        public void SetAsHero()
        {
            SetType('H', ConsoleColor.White);
        }

        public void SetAsProjectile()
        {
            SetType('*', ConsoleColor.Cyan);
        }

        public void Recieve(RigidBodyCreated rigidBodyCreated)
        {
            var position = rigidBodyCreated.Position;
            var point = position.ToPoint();

            _lastPosition = point;
            _transformedLastPosition = Transform(point);
        }

        public void Recieve(Moved moved)
        {
            if (!_character.HasValue || !_color.HasValue)
            {
                throw new InvalidOperationException("Rigid body type not set");
            }

            var newPosition = moved.NewPosition.ToPoint();
            var position = Transform(newPosition);

            if (position == _transformedLastPosition)
            {
                return;
            }

            _consoleRenderer.Render(position, _character.Value, _color.Value);
            Remove();

            _lastPosition = newPosition;
            _transformedLastPosition = position;
        }

        public void Remove()
        {
            var c = _map[_lastPosition] == Tile.Solid ? '?' : '·';

            _consoleRenderer.Render(_transformedLastPosition, c, ConsoleColor.DarkGray);
        }

        private void SetType(char character, ConsoleColor color)
        {
            _character = character;
            _color = color;
            _consoleRenderer.Render(_transformedLastPosition, _character.Value, _color.Value);
        }

        private Point Transform(Point position)
        {
            var x = position.X;
            var y = position.Y;
            var cx = 1 + x;
            var cy = _map.Dimensions.Height - y;

            return new Point(cx, cy);
        }

        private readonly Map<Tile> _map;
        private readonly ConsoleRenderer _consoleRenderer;

        private Point _transformedLastPosition;
        private char? _character;
        private ConsoleColor? _color;
        private Point _lastPosition;
    }
}