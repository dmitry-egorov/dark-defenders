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
        private Point _lastPlayerPosition;
        private long _eventCount = 0L;

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

        private static Point Transform(Vector spawnPosition)
        {
            var x = 39 + (int)(spawnPosition.X * 39);
            var y = 23;

            return new Point(x, y);
        }

        private static void Render(Point position, char c)
        {
            System.Console.SetCursorPosition(position.X, position.Y);
            System.Console.Write(c);
        }

        private void RenderTerrain()
        {
            RenderHorizontalLine(0);
            RenderHorizontalLine(24);

            RenderVerticalLine(0);
            RenderVerticalLine(78);
        }

        private static void RenderHorizontalLine(int top)
        {
            for (var i = 1; i < 78; i++)
            {
                System.Console.SetCursorPosition(i, top);
                System.Console.Write('-');
            }
        }

        private static void RenderVerticalLine(int left)
        {
            for (var i = 1; i < 24; i++)
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