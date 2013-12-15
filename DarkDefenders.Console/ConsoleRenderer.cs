using System.Drawing;
using DarkDefenders.Domain.Players;
using DarkDefenders.Domain.Players.Events;
using DarkDefenders.Domain.Terrains;
using DarkDefenders.Domain.Terrains.Events;
using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.Console
{
    internal class ConsoleRenderer: IPlayerEventsReciever, ITerrainEventsReciever
    {
        private readonly int _width;
        private readonly int _height;

        private Point _lastPlayerPosition;
        private long _eventCount;

        public ConsoleRenderer(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void Initialize()
        {
            System.Console.BufferWidth = System.Console.WindowWidth = _width;
            System.Console.BufferHeight = System.Console.WindowHeight = _height;

            System.Console.CursorVisible = false;
        }

        public void Apply(IEvent @event)
        {
            ((dynamic) @event).ApplyTo((dynamic) this);

            _eventCount++;
            RenderEventCount();
        }

        public void Apply(PlayerCreated playerCreated)
        {
            var position = Transform(playerCreated.SpawnPosition);

            Render(position, '@');

            _lastPlayerPosition = position;
        }

        public void Apply(PlayersDesiredOrientationIsSet playersDesiredOrientationIsSet)
        {
            
        }

        public void Apply(PlayerMoved playerMoved)
        {
            var position = Transform(playerMoved.NewPosition);
            if (position == _lastPlayerPosition)
            {
                return;
            }

            Render(_lastPlayerPosition, ' ');
            Render(position, '@');

            _lastPlayerPosition = position;
        }

        public void Apply(TerrainCreated terrainCreated)
        {
            RenderTerrain();
        }

        private void RenderEventCount()
        {
            System.Console.SetCursorPosition(0, 0);
            System.Console.Write(_eventCount);
        }

        private Point Transform(Vector spawnPosition)
        {
            var x = (int)(_width * (1 + spawnPosition.X) / 2);
            var y = _height - 2;

            return new Point(x, y);
        }

        private static void Render(Point position, char c)
        {
            System.Console.SetCursorPosition(position.X, position.Y);
            System.Console.Write(c);
        }

        private void RenderTerrain()
        {
            RenderHorizontalLine(0, 1, _width - 2);
            RenderHorizontalLine(_height - 1, 1, _width - 2);
            RenderVerticalLine(1, 0, _height - 2);
            RenderVerticalLine(1, _width - 1, _height - 1);
        }

        private static void RenderHorizontalLine(int top, int left, int length)
        {
            var end = left + length;
            for (var i = left; i < end; i++)
            {
                System.Console.SetCursorPosition(i, top);
                System.Console.Write('-');
            }
        }

        private static void RenderVerticalLine(int top, int left, int length)
        {
            for (var i = top; i < length; i++)
            {
                System.Console.SetCursorPosition(left, i);
                System.Console.Write('|');
            }
        }

        public void RenderFps(double fps)
        {
            System.Console.SetCursorPosition(70, 0);
            System.Console.Write(fps);
        }
    }
}