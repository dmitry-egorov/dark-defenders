using System.Collections.Generic;
using System.Linq;
using Infrastructure.Util;
using MoreLinq;
using Wintellect.PowerCollections;

namespace Infrastructure.DDDES.Implementations
{
    public class EventStore : IEventStore
    {
        private readonly MultiDictionary<Identity, IEvent> _eventsMap = new MultiDictionary<Identity,IEvent>(true);
        private readonly List<IEvent> _allEvents = new List<IEvent>();

        public IEnumerable<IEvent> GetById(Identity id)
        {
            return _eventsMap[id];
        }

        public IEnumerable<IEvent> GetAll()
        {
            return _allEvents;
        }

        public void Append(IEvent e)
        {
            _allEvents.Add(e);
            _eventsMap.Add(e.RootId, e);
        }

        public void Append(IEnumerable<IEvent> events)
        {
            var readOnly = events.AsReadOnly();
            
            _allEvents.AddRange(readOnly);

            readOnly
                .GroupBy(x => x.RootId)
                .ForEach(x =>
                {
                    _eventsMap.AddMany(x.Key, x);
                });
        }

        public IEnumerable<Identity> GetAllIds()
        {
            return _eventsMap.Keys;
        }
    }
}