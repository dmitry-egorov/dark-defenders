using System;
using DarkDefenders.Domain.Clocks;

namespace DarkDefenders.Domain.Other
{
    public class Cooldown
    {
        private readonly Clock _clock;
        private readonly TimeSpan _cooldownDelay;

        private TimeSpan _activationTime = TimeSpan.Zero;

        public Cooldown(Clock clock, TimeSpan cooldownDelay)
        {
            _clock = clock;
            _cooldownDelay = cooldownDelay;
        }

        public void SetLastActivationTime(TimeSpan activationTime)
        {
            _activationTime = activationTime;
        }

        public bool IsInEffect()
        {
            return _clock.GetCurrentTime() - _activationTime < _cooldownDelay;
        }
    }
}