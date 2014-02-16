using System;
using DarkDefenders.Domain.Model.Entities;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Other
{
    [UsedImplicitly]
    public class CooldownFactory
    {
        private readonly Clock _clock;

        public CooldownFactory(Clock clock)
        {
            _clock = clock;
        }

        public Cooldown Create(TimeSpan cooldownDelay)
        {
            return new Cooldown(_clock, cooldownDelay);
        }
    }
}