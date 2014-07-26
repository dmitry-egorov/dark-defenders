using System.Windows.Forms;

namespace DarkDefenders.ConsoleClient
{
    public class KeyboardState
    {
        private readonly byte[] _bCharData;

        internal KeyboardState(byte[] bCharData)
        {
            _bCharData = bCharData;
        }

        //NOTE: not working :(
        public bool IsKeyDown(Keys key)
        {
            var code = NativeKeyboard.GetVirtualKeyCode(key);
            return (_bCharData[code] & NativeKeyboard.KeyPressed) != 0;
        }
    }
}