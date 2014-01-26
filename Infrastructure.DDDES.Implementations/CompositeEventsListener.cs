using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations
{
    public class CompositeEventsListener<TDomainEvent> : IEventsListener<TDomainEvent>
    {
        private readonly ReadOnlyCollection<IEventsListener<TDomainEvent>> _listeners;

        public CompositeEventsListener(params IEventsListener<TDomainEvent>[] listeners) : this(listeners.AsEnumerable())
        {
            
        }

        public CompositeEventsListener(IEnumerable<IEventsListener<TDomainEvent>> linsteners)
        {
            _listeners = linsteners.ShouldNotBeNull("listeners").AsReadOnly();
        }

        public void Recieve(TDomainEvent entityEvent)
        {
            foreach (var linstener in _listeners)
            {
                linstener.Recieve(entityEvent);
            }
        }
    }
}