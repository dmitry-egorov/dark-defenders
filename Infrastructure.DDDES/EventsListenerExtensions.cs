using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public static class EventsListenerExtensions
    {
        public static void Recieve<TDomainEvent>(this IEventsListener<TDomainEvent> listener, IEnumerable<TDomainEvent> events)
        {
            foreach (var domainEvent in events)
            {
                listener.Recieve(domainEvent);
            }
        }
    }
}