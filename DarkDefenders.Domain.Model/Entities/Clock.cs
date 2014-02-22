using System;
using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES.Implementations.Domain;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities
{
    [UsedImplicitly]
    public class Clock : Entity<Clock, IClockEvents>, IClockEvents
    {
        private TimeSpan _currentTime;

        public void Create()
        {
            CreationEvent(r => r.Created());
        }

        public void UpdateTime(TimeSpan elapsed)
        {
            var newTime = _currentTime + elapsed;
            Event(x => x.TimeChanged(newTime));
        }

        public TimeSpan GetCurrentTime()
        {
            return _currentTime;
        }

        void IClockEvents.Created()
        {
            _currentTime = TimeSpan.Zero;
        }

        void IClockEvents.TimeChanged(TimeSpan newTime)
        {
            _currentTime = newTime;
        }
    }
}