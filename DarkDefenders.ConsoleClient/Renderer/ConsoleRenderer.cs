using System;
using System.Drawing;
using System.Linq;

namespace DarkDefenders.ConsoleClient.Renderer
{
    public class ConsoleRenderer : IConsoleRenderer
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
            Console.BufferWidth = Console.WindowWidth = Math.Max(_width, 80);
            Console.BufferHeight = Console.WindowHeight = Math.Max(_height, 50);

            Console.CursorVisible = false;
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

        public void Render(int x, int y, char c, ConsoleColor color = ConsoleColor.White)
        {
            if (IsOutOfScreen(x, y))
            {
                return;
            }

            var previousColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(c);
            Console.ForegroundColor = previousColor;
        }

        private bool IsOutOfScreen(int x, int y)
        {
            return x < 0 || x >= _width || y < 0 || y >= _height;
        }

        public void Render(int left, int top, char[] line, ConsoleColor color = ConsoleColor.White)
        {
            
            var previousColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.SetCursorPosition(left, top);
            Console.Write(line);
            Console.ForegroundColor = previousColor;
        }

        public void Render(Point position, char c, ConsoleColor color = ConsoleColor.White)
        {
            Render(position.X, position.Y, c, color);
        }

        public void Render(int left, int top, string str)
        {
            Console.SetCursorPosition(left, top);
            Console.Write(str);
        }
    }
}