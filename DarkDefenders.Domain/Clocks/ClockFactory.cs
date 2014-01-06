using System.Collections.Generic;
using DarkDefenders.Domain.Clocks.Events;
using DarkDefenders.Domain.Events;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Clocks
{
    public class ClockFactory: RootFactory<ClockId, Clock, ClockCreated>
    {
        public ClockFactory(IRepository<ClockId, Clock> repository) : base(repository)
        {
        }

        public IEnumerable<IDomainEvent> Create(ClockId clockId)
        {
            AssertDoesntExist(clockId);
            yield return new ClockCreated(clockId);
        }

        protected override Clock Handle(ClockCreated creationEvent)
        {
            return new Clock(creationEvent.RootId);
        }
    }
}