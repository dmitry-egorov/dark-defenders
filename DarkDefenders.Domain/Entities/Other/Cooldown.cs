using System;
using DarkDefenders.Domain.Entities.Clocks;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Other
{
    internal class Cooldown
    {
        private readonly IContainer<Clock> _clockContainer;
        private readonly TimeSpan _cooldownDelay;

        private TimeSpan _activationTime = TimeSpan.Zero;

        public Cooldown(IContainer<Clock> clockContainer, TimeSpan cooldownDelay)
        {
            _clockContainer = clockContainer;
            _cooldownDelay = cooldownDelay;
        }

        public void SetLastActivationTime(TimeSpan activationTime)
        {
            _activationTime = activationTime;
        }

        public bool IsInEffect()
        {
            var clock = _clockContainer.Item;
            return clock.GetCurrentTime() - _activationTime < _cooldownDelay;
        }
    }
}