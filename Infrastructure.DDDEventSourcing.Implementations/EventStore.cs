using System.Collections.Generic;
using Infrastructure.DDDEventSourcing.Domain;
using Wintellect.PowerCollections;

namespace Infrastructure.DDDEventSourcing.Implementations
{
    public class EventStore : IEventStore
    {
        private readonly MultiDictionary<Identity, IEvent> _events = new MultiDictionary<Identity,IEvent>(true);

        public IEnumerable<IEvent> Get(Identity id)
        {
            return _events[id];
        }

        public void Append(Identity id, IEnumerable<IEvent> events)
        {
            _events.AddMany(id, events);
        }
    }
}