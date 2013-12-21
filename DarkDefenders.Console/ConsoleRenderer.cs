using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using DarkDefenders.Domain.Players;
using DarkDefenders.Domain.Players.Events;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.RigidBodies.Events;
using DarkDefenders.Domain.Worlds;
using DarkDefenders.Domain.Worlds.Events;
using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.Console
{
    internal class ConsoleRenderer: IPlayerEventsReciever, IRigidBodyEventsReciever, IWorldEventsReciever, IEventsLinstener
    {
        private readonly int _width;
        private readonly int _height;

        private Point _lastPlayerPosition;

        public static ConsoleRenderer InitializeNew()
        {
            var renderer = new ConsoleRenderer(100, 40);

            renderer.Initialize();
            return renderer;
        }

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

        public void Apply(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                Apply(@event);
            }
        }

        public void Apply(WorldCreated worldCreated)
        {
            RenderWorld();
        }

        public void Apply(PlayerCreated playerCreated)
        {
            
        }

        public void Apply(MovementForceChanged movementForceChanged)
        {
            RenderMovementForce(movementForceChanged);
        }

        public void Apply(RigidBodyCreated rigidBodyCreated)
        {
            var position = Transform(rigidBodyCreated.SpawnPosition);

            Render(position, '@');

            _lastPlayerPosition = position;
        }

        public void Apply(Moved playerMoved)
        {
            RenderPosition(playerMoved.NewPosition);

            var position = Transform(playerMoved.NewPosition);


            if (position == _lastPlayerPosition)
            {
                return;
            }

            Render(_lastPlayerPosition, ' ');
            Render(position, '@');

            _lastPlayerPosition = position;
        }

        public void Apply(Accelerated playerAccelerated)
        {
            RenderVelocity(playerAccelerated.NewMomentum);
        }

        public void Apply(ExternalForceChanged externalForceChanged)
        {
        }

        public void RenderFps(double fps)
        {
            var fpsString = fps.ToString(CultureInfo.InvariantCulture);
            RenderFloatRight(fpsString, 0, 8);
        }

        public void RenderAverageEventsCount(double averageEventsCount)
        {
            Render(0, 1, "     ");
            Render(0, 1, averageEventsCount.ToString(CultureInfo.InvariantCulture));
        }

        public void RenderEventsCount(long eventsCount)
        {
            Render(0, 0, eventsCount.ToString(CultureInfo.InvariantCulture));
        }

        private void RenderMovementForce(MovementForceChanged movementForceChanged)
        {
            var text = "d: " + movementForceChanged.MovementForce;
            RenderFloatRight(text, 3, 30);
        }

        private void Apply(IEvent @event)
        {
            ((dynamic) @event).ApplyTo((dynamic) this);
        }

        private Point Transform(Vector spawnPosition)
        {
            var x = spawnPosition.X;
            var y = spawnPosition.Y;
            var cx = 1 + (int)((_width - 2) * (1 + x) / 2);
            var cy = 1 + (int)((_height - 2) * (1 - y));

            return new Point(cx, cy);
        }

        private void RenderWorld()
        {
            RenderHorizontalLine(0, 1, _width - 2);
            RenderHorizontalLine(_height - 1, 1, _width - 2);
            RenderVerticalLine(1, 0, _height - 1);
            RenderVerticalLine(1, _width - 1, _height - 1);
        }

        private void RenderPosition(Vector position)
        {
            RenderFloatRight("p: " + position.ToString("0.00"), 1, 30);
        }

        private void RenderVelocity(Vector velocity)
        {
            RenderFloatRight("v: " + velocity.ToString("0.00"), 2, 30);
        }

        private static void RenderHorizontalLine(int top, int left, int length)
        {
            var end = left + length;
            for (var i = left; i < end; i++)
            {
                Render(i, top, '-');
            }
        }

        private static void RenderVerticalLine(int top, int left, int length)
        {
            for (var i = top; i < length; i++)
            {
                Render(left, i, '|');
            }
        }

        private void RenderFloatRight(string text, int top, int max)
        {
            var spaces = new string(Enumerable.Repeat(' ', max).ToArray());

            Render(_width - max, top, spaces);
            Render(_width - text.Length, top, text);
        }

        private static void Render(int x, int y, char c)
        {
            System.Console.SetCursorPosition(x, y);
            System.Console.Write(c);
        }

        private static void Render(Point position, char c)
        {
            System.Console.SetCursorPosition(position.X, position.Y);
            System.Console.Write(c);
        }

        private static void Render(int left, int top, string str)
        {
            System.Console.SetCursorPosition(left, top);
            System.Console.Write(str);
        }
    }
}