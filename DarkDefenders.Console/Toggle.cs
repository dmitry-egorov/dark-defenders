using System;

namespace DarkDefenders.Console
{
    internal class Toggle
    {
        private bool _isToggled;
        private bool _canToggle = true;

        public Toggle(bool isToggled)
        {
            _isToggled = isToggled;
        }

        public void WhenToggled(Action action) 
        {
            if (_isToggled)
            {
                action();
            }
        }
 
        public bool IsToggled { get { return _isToggled; } }

        public void Tick(bool isPressed)
        {
            if (!isPressed)
            {
                _canToggle = true;
                return;
            }
            
            if (!_canToggle)
            {
                return;
            }

            _isToggled = !_isToggled;
            _canToggle = false;
        }
    }
}