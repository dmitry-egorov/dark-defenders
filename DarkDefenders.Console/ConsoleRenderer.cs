using System;
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
            System.Console.BufferWidth = System.Console.WindowWidth = Math.Max(_width, 80);
            System.Console.BufferHeight = System.Console.WindowHeight = Math.Max(_height, 50);

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

        public void Render(int x, int y, char c, ConsoleColor color = ConsoleColor.White)
        {
            if (IsOutOfScreen(x, y))
            {
                return;
            }

            var previousColor = System.Console.ForegroundColor;

            System.Console.ForegroundColor = color;
            System.Console.SetCursorPosition(x, y);
            System.Console.Write(c);
            System.Console.ForegroundColor = previousColor;
        }

        private bool IsOutOfScreen(int x, int y)
        {
            return x < 0 || x >= _width || y < 0 || y >= _height;
        }

        public void Render(int left, int top, char[] line, ConsoleColor color = ConsoleColor.White)
        {
            
            var previousColor = System.Console.ForegroundColor;

            System.Console.ForegroundColor = color;
            System.Console.SetCursorPosition(left, top);
            System.Console.Write(line);
            System.Console.ForegroundColor = previousColor;
        }

        public void Render(Point position, char c, ConsoleColor color = ConsoleColor.White)
        {
            Render(position.X, position.Y, c, color);
        }

        public void Render(int left, int top, string str)
        {
            System.Console.SetCursorPosition(left, top);
            System.Console.Write(str);
        }
    }
}