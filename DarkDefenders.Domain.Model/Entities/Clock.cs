using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;
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

        public Clock(IClockEvents external, IStorage<Clock> storage) : base(external, storage)
        {
        }

        public IEnumerable<IEvent> Create()
        {
            yield return CreationEvent(r => r.Created());
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
            yield return Event(x => x.TimeChanged(newTime));
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

        void IEntityEvents.Destroyed()
        {
        }
    }
}