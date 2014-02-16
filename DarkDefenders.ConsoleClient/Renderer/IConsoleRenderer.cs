using System;
using System.Drawing;

namespace DarkDefenders.ConsoleClient.Renderer
{
    public interface IConsoleRenderer
    {
        void InitializeScreen();
        void RenderHorizontalLine(int top, int left, int length);
        void RenderVerticalLine(int top, int left, int length);
        void RenderFloatRight(string text, int top, int max, int width);
        void Render(int x, int y, char c, ConsoleColor color = ConsoleColor.White);
        void Render(int left, int top, char[] line, ConsoleColor color = ConsoleColor.White);
        void Render(Point position, char c, ConsoleColor color = ConsoleColor.White);
        void Render(int left, int top, string str);
    }
}