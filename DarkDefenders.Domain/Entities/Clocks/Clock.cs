using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DarkDefenders.Domain.Entities.Clocks.Events;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Physics;

namespace DarkDefenders.Domain.Entities.Clocks
{
    public class Clock: Entity<Clock>
    {
        private Seconds _elapsedSeconds;
        private TimeSpan _currentTime;

        internal Clock()
        {
            _currentTime = TimeSpan.Zero;
            _elapsedSeconds = Seconds.Zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TimeSpan GetCurrentTime()
        {
            return _currentTime;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Seconds GetElapsedSeconds()
        {
            return _elapsedSeconds;
        }

        public IEnumerable<IEvent> UpdateTime(TimeSpan elapsed)
        {
            var newTime = _currentTime + elapsed;
            yield return new TimeChanged(this, newTime);
        }

        internal void TimeChanged(TimeSpan newTime)
        {
            _elapsedSeconds = (newTime - _currentTime).ToSeconds();
            _currentTime = newTime;
        }
    }
}