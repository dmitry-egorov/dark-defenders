using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations
{
    public class CompositeEventsListener<TDomainEvent> : IEventsLinstener<TDomainEvent>
    {
        private readonly ReadOnlyCollection<IEventsLinstener<TDomainEvent>> _listeners;

        public CompositeEventsListener(params IEventsLinstener<TDomainEvent>[] linsteners) : this(linsteners.AsEnumerable())
        {
            
        }

        public CompositeEventsListener(IEnumerable<IEventsLinstener<TDomainEvent>> linsteners)
        {
            _listeners = linsteners.ShouldNotBeNull("listeners").AsReadOnly();
        }

        public void Recieve(IEnumerable<TDomainEvent> events)
        {
            foreach (var linstener in _listeners)
            {
                linstener.Recieve(events);//TODO: as read only?
            }
        }
    }
}