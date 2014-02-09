using System;
using System.Drawing;
using DarkDefenders.ConsoleClient.Renderer;
using DarkDefenders.Domain.Data.Entities.RigidBodies;
using DarkDefenders.Domain.Data.Other;
using Infrastructure.Math;

namespace DarkDefenders.ConsoleClient.ViewModels
{
    internal class RigidBodyViewModel
    {
        public RigidBodyViewModel(Map<Tile> map, IConsoleRenderer consoleRenderer)
        {
            _map = map;
            _consoleRenderer = consoleRenderer;
        }

        public void SetAsPlayersAvatar()
        {
            SetType('@', ConsoleColor.Cyan);
            _renderImmediately = true;
        }

        public void SetAsHero()
        {
            SetType('H', ConsoleColor.White);
        }

        public void SetAsProjectile()
        {
            SetType('*', ConsoleColor.Cyan);
            _renderImmediately = true;
        }

        public void Render()
        {
            if (_renderImmediately)
            {
                return;
            }

            RenderInternal();
        }

        private void RenderInternal()
        {
            if (!_character.HasValue || !_color.HasValue)
            {
                throw new InvalidOperationException("Rigid body type not set");
            }

            var newRenderingPosition = _currentPosition;

            if (newRenderingPosition == _lastRenderingPosition)
            {
                return;
            }

            var transformedPosition = Transform(newRenderingPosition);
            _consoleRenderer.Render(transformedPosition, _character.Value, _color.Value);
            Remove();

            _lastRenderingPosition = newRenderingPosition;
        }

        public void Recieve(RigidBodyCreatedData rigidBodyCreated)
        {
            var position = rigidBodyCreated.Properties.Position;
            var point = position.ToVector().ToPoint();

            _currentPosition = point;
        }

        public void SetNewPosition(Vector newPosition)
        {
            _currentPosition = newPosition.ToPoint();
            if (_renderImmediately)
            {
                RenderInternal();
            }
        }

        public void Remove()
        {
            var position = _lastRenderingPosition;

            var c = _map[position] == Tile.Solid ? '?' : '·';

            var transformedPosition = Transform(position);

            _consoleRenderer.Render(transformedPosition, c, ConsoleColor.DarkGray);
        }

        private void SetType(char character, ConsoleColor color)
        {
            _character = character;
            _color = color;
            if (_renderImmediately)
            {
                RenderInternal();
            }
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
        private readonly IConsoleRenderer _consoleRenderer;

        private char? _character;
        private ConsoleColor? _color;
        private Point _currentPosition;
        private Point _lastRenderingPosition;
        private bool _renderImmediately;
    }
}