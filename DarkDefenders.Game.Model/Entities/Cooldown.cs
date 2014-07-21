using System;
using DarkDefenders.Game.Model.Events;
using Infrastructure.DDDES.Implementations.Domain;
using JetBrains.Annotations;

namespace DarkDefenders.Game.Model.Entities
{
    [UsedImplicitly]
    public class Cooldown : Entity<Cooldown, ICooldownEvents>, ICooldownEvents
    {
        private readonly Clock _clock;
        
        private TimeSpan _cooldownDelay;

        private TimeSpan _activationTime = TimeSpan.Zero;

        public Cooldown(Clock clock)
        {
            _clock = clock;
        }

        public void Create(TimeSpan cooldownDelay)
        {
            CreationEvent(x => x.Created(cooldownDelay));
        }

        public void TryActivate(Action action)
        {
            var currentTime = _clock.GetCurrentTime();

            if (currentTime - _activationTime < _cooldownDelay)
            {
                return;
            }

            action();

            Event(x => x.Activated(currentTime));
        }

        void ICooldownEvents.Created(TimeSpan cooldownDelay)
        {
            _cooldownDelay = cooldownDelay;
        }

        void ICooldownEvents.Activated(TimeSpan activationTime)
        {
            _activationTime = activationTime;
        }
    }
}