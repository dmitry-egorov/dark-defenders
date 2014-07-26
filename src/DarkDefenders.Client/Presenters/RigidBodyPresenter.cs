using System;
using System.Drawing;
using DarkDefenders.Client.Renderers;
using DarkDefenders.Game.Model.Other;
using DarkDefenders.Remote.Model;
using Infrastructure.Math;

namespace DarkDefenders.Client.Presenters
{
    internal class RigidBodyPresenter
    {
        private readonly Map<Tile> _map;
        private readonly IConsoleRenderer _consoleRenderer;

        private char? _character;
        private ConsoleColor? _color;
        private Point _currentPosition;
        private Point _lastRenderingPosition;
        private bool _renderImmediately;

        public RigidBodyPresenter(Map<Tile> map, IConsoleRenderer consoleRenderer)
        {
            _map = map;
            _consoleRenderer = consoleRenderer;
        }

        public RemoteEntityType Type { get; private set; }

        public void Render()
        {
            if (_renderImmediately)
            {
                return;
            }

            RenderInternal();
        }

        public void RigidBodyCreated(Vector position, RemoteEntityType type)
        {
            switch (type)
            {
                case RemoteEntityType.Player:
                    SetAsPlayersAvatar();
                    break;
                case RemoteEntityType.Hero:
                    SetAsHero();
                    break;
                case RemoteEntityType.Projectile:
                    SetAsProjectile();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }

            Type = type;

            SetNewPosition(position);
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

        private void SetAsPlayersAvatar()
        {
            SetType('@', ConsoleColor.Cyan);
            _renderImmediately = true;
        }

        private void SetAsHero()
        {
            SetType('H', ConsoleColor.White);
        }

        private void SetAsProjectile()
        {
            SetType('*', ConsoleColor.Cyan);
            _renderImmediately = true;
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
    }
}