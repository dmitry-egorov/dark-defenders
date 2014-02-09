﻿using System;
using DarkDefenders.Domain.Entities.Clocks;

namespace DarkDefenders.Domain.Entities.Other
{
    internal class Cooldown
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
            var currentTime = _clock.GetCurrentTime();

            return currentTime - _activationTime < _cooldownDelay;
        }
    }
}