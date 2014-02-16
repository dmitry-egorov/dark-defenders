using System.Collections.Generic;
using Infrastructure.Util;
using JetBrains.Annotations;

namespace Infrastructure.DDDES.Implementations
{
    [UsedImplicitly]
    public class EventsProcessor : IEventsProcessor
    {
        public void Process(IEnumerable<IEvent> events)
        {
            var all = events.AsReadOnly();

            all.ApplyAll();
        }
    }
}