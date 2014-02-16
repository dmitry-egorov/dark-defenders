using System;
using System.Drawing;

namespace DarkDefenders.ConsoleClient.Renderers
{
    class EmptyConsoleRenderer : IConsoleRenderer
    {
        public void InitializeScreen()
        {
        }

        public void RenderHorizontalLine(int top, int left, int length)
        {
        }

        public void RenderVerticalLine(int top, int left, int length)
        {
        }

        public void RenderFloatRight(string text, int top, int max, int width)
        {
        }

        public void Render(int x, int y, char c, ConsoleColor color = ConsoleColor.White)
        {
        }

        public void Render(int left, int top, char[] line, ConsoleColor color = ConsoleColor.White)
        {
        }

        public void Render(Point position, char c, ConsoleColor color = ConsoleColor.White)
        {
        }

        public void Render(int left, int top, string str)
        {
        }
    }
}