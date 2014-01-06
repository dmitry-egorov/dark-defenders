using System;

namespace DarkDefenders.Console
{
    internal class OnOffSwitch
    {
        public bool IsOn { get { return _isOn; } }

        public OnOffSwitch(bool isOn)
        {
            _isOn = isOn;
            _button = new Button();
        }

        public void WhenOn(Action action) 
        {
            if (_isOn)
            {
                action();
            }
        }

        public void State(bool isPressed)
        {
            _button.State(isPressed, () =>
            {
                _isOn = !_isOn;
            });
        }

        private readonly Button _button;

        private bool _isOn;
    }
}