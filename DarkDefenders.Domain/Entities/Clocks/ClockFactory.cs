using System.Collections.Generic;
using DarkDefenders.Domain.Entities.Clocks.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Clocks
{
    internal class ClockFactory
    {
        private readonly IStorage<Clock> _storage;

        public ClockFactory(IStorage<Clock> storage)
        {
            _storage = storage;
        }

        public IEnumerable<IEvent> Create()
        {
            yield return new ClockCreated(_storage);
        }
    }
}