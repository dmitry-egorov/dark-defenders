using System.Collections.Generic;
using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations
{
    public class EventStore : IEventStore
    {
        private readonly List<IEvent> _allEvents = new List<IEvent>();

        public IEnumerable<IEvent> GetAll()
        {
            return _allEvents;
        }

        public void Append(IEnumerable<IEvent> events)
        {
            var readOnly = events.AsReadOnly();
            
            _allEvents.AddRange(readOnly);
        }
    }
}