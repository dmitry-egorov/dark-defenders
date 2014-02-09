using System.Collections.Generic;
using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Entities.Clocks.Events;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Factories
{
    internal class ClockFactory: Factory<Clock>
    {
        public ClockFactory(IStorage<Clock> storage)
            : base(storage)
        {
        }

        public ICreation<Clock> Create()
        {
            return GetCreation(YieldEvents);
        }

        private static IEnumerable<IEvent> YieldEvents(IStorage<Clock> storage)
        {
            yield return new ClockCreated(storage);
        }
    }
}