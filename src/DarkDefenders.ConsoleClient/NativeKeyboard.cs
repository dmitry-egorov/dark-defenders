using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DarkDefenders.ConsoleClient
{
    /// <summary>
    /// Provides keyboard access.
    /// </summary>
    public static class NativeKeyboard
    {
        /// <summary>
        /// A positional bit flag indicating the part of a key state denoting
        /// key pressed.
        /// </summary>
        internal const byte KeyPressed = 0x80;

        /// <summary>
        /// Returns a value indicating if a given key is pressed.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>
        /// <c>true</c> if the key is pressed, otherwise <c>false</c>.
        /// </returns>
        public static bool IsKeyDown(Keys key)
        {
            var code = GetVirtualKeyCode(key);
            return (GetKeyState(code) & KeyPressed) != 0;
        }

        public static KeyboardState GetKeyboardState()
        {
            var bCharData = new byte[256];
            GetKeyboardState(bCharData);

            return new KeyboardState(bCharData);
        }

        internal static byte GetVirtualKeyCode(Keys key)
        {
            var value = (int)key;
            return (byte)(value & 0xFF);
        }

        /// <summary>
        /// Gets the key state of a key.
        /// </summary>
        /// <param name="key">Virtuak-key code for key.</param>
        /// <returns>The state of the key.</returns>
        [DllImport("user32.dll")]
        private static extern short GetKeyState(int key);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetKeyboardState(byte[] lpKeyState);
    }
}