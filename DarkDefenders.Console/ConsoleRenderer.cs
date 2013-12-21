using System.Drawing;
using System.Linq;

namespace DarkDefenders.Console
{
    public static class ConsoleRenderer
    {
        public static void RenderHorizontalLine(int top, int left, int length)
        {
            var end = left + length;
            for (var i = left; i < end; i++)
            {
                Render(i, top, '-');
            }
        }

        public static void RenderVerticalLine(int top, int left, int length)
        {
            for (var i = top; i < length; i++)
            {
                Render(left, i, '|');
            }
        }

        public static void RenderFloatRight(string text, int top, int max, int width)
        {
            var spaces = new string(Enumerable.Repeat(' ', max).ToArray());

            Render(width - max, top, spaces);
            Render(width - text.Length, top, text);
        }

        public static void Render(int x, int y, char c)
        {
            System.Console.SetCursorPosition(x, y);
            System.Console.Write(c);
        }

        public static void Render(Point position, char c)
        {
            System.Console.SetCursorPosition(position.X, position.Y);
            System.Console.Write(c);
        }

        public static void Render(int left, int top, string str)
        {
            System.Console.SetCursorPosition(left, top);
            System.Console.Write(str);
        }
    }
}