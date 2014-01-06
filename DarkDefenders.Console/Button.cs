using System;

namespace DarkDefenders.Console
{
    internal class Button
    {
        public void State(bool isPressed, Action onKeyDown)
        {
            if (!isPressed)
            {
                _canPress = true;
                return;
            }

            if (!_canPress)
            {
                return;
            }

            _canPress = false;

            onKeyDown();
        }

        private bool _canPress = true;
    }
}