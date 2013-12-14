using System.Collections.Generic;
using Infrastructure.DDDEventSourcing.Domain;
using Wintellect.PowerCollections;

namespace Infrastructure.DDDEventSourcing.Implementations
{
    public class EventStore : IEventStore
    {
        private readonly MultiDictionary<Identity, IEventMarker> _events = new MultiDictionary<Identity,IEventMarker>(true);

        public IEnumerable<IEventMarker> Get(Identity id)
        {
            return _events[id];
        }

        public void Append(Identity id, IEnumerable<IEventMarker> events)
        {
            _events.AddMany(id, events);
        }
    }
}