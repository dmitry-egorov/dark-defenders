using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DarkDefenders.Domain.Clocks.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math.Physics;

namespace DarkDefenders.Domain.Clocks
{
    public class Clock: RootBase<ClockId, IClockEventsReciever, IClockEvent>, IClockEventsReciever
    {
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

        public void Recieve(ClockTimeUpdated clockTimeUpdated)
        {
            _elapsedSeconds = (clockTimeUpdated.NewTime - _currentTime).ToSeconds();
            _currentTime = clockTimeUpdated.NewTime;
        }

        internal Clock(ClockId id) : base(id)
        {
            _currentTime = TimeSpan.Zero;
        }

        public IEnumerable<IClockEvent> UpdateTime(TimeSpan elapsed)
        {
            var newTime = _currentTime + elapsed;

            yield return new ClockTimeUpdated(Id, newTime);
        }

        private Seconds _elapsedSeconds;
        private TimeSpan _currentTime;
    }
}