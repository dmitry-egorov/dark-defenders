using System.Drawing;
using System.Linq;

namespace DarkDefenders.Console
{
    public class ConsoleRenderer
    {
        private readonly int _width;
        private readonly int _height;

        public ConsoleRenderer(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void InitializeScreen()
        {
            System.Console.BufferWidth = System.Console.WindowWidth = _width;
            System.Console.BufferHeight = System.Console.WindowHeight = _height;

            System.Console.CursorVisible = false;
        }

        public void RenderHorizontalLine(int top, int left, int length)
        {
            var end = left + length;
            for (var i = left; i < end; i++)
            {
                Render(i, top, '-');
            }
        }

        public void RenderVerticalLine(int top, int left, int length)
        {
            for (var i = top; i < length; i++)
            {
                Render(left, i, '|');
            }
        }

        public void RenderFloatRight(string text, int top, int max, int width)
        {
            var spaces = new string(Enumerable.Repeat(' ', max).ToArray());

            Render(width - max, top, spaces);
            Render(width - text.Length, top, text);
        }

        public void Render(int x, int y, char c)
        {
            if (IsOutOfScreen(x, y))
            {
                return;
            }

            System.Console.SetCursorPosition(x, y);
            System.Console.Write(c);
        }

        private bool IsOutOfScreen(int x, int y)
        {
            return x < 0 || x >= _width || y < 0 || y >= _height;
        }

        public void Render(Point position, char c)
        {
            Render(position.X, position.Y, c);
        }

        public void Render(int left, int top, string str)
        {
            System.Console.SetCursorPosition(left, top);
            System.Console.Write(str);
        }
    }
}