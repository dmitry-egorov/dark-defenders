using System.Collections.Generic;

namespace Infrastructure.DDDES.Implementations
{
    public static class EventsExtensions
    {
        public static void ApplyAll(this IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                @event.Apply();
            }
        }
    }
}