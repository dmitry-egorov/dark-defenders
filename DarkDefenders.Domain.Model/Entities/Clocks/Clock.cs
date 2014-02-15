using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DarkDefenders.Domain.Model.Entities.Clocks.Events;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Physics;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities.Clocks
{
    [UsedImplicitly]
    public class Clock : Entity<Clock>
    {
        private readonly IStorage<Clock> _storage;

        private Seconds _elapsedSeconds;
        private TimeSpan _currentTime;

        public Clock(IStorage<Clock> storage)
        {
            _storage = storage;
        }

        public IEnumerable<IEvent> Create()
        {
            yield return new ClockCreated(this, _storage);
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

        internal void Created()
        {
            _currentTime = TimeSpan.Zero;
            _elapsedSeconds = Seconds.Zero;
        }

        internal void TimeChanged(TimeSpan newTime)
        {
            _elapsedSeconds = (newTime - _currentTime).ToSeconds();
            _currentTime = newTime;
        }
    }
}