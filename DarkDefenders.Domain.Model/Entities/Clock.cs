using System;
using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Physics;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities
{
    [UsedImplicitly]
    public class Clock : Entity<Clock, IClockEvents>, IClockEvents
    {
        private Seconds _elapsedSeconds;
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

        public Seconds GetElapsedSeconds()
        {
            return _elapsedSeconds;
        }

        void IClockEvents.Created()
        {
            _currentTime = TimeSpan.Zero;
            _elapsedSeconds = Seconds.Zero;
        }

        void IClockEvents.TimeChanged(TimeSpan newTime)
        {
            _elapsedSeconds = (newTime - _currentTime).ToSeconds();
            _currentTime = newTime;
        }
    }
}