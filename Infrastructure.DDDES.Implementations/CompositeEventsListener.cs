using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations
{
    public class CompositeEventsListener : IEventsLinstener
    {
        private readonly ReadOnlyCollection<IEventsLinstener> _listeners;

        public CompositeEventsListener(params IEventsLinstener[] linsteners) : this(linsteners.AsEnumerable())
        {
            
        }

        public CompositeEventsListener(IEnumerable<IEventsLinstener> linsteners)
        {
            _listeners = linsteners.ShouldNotBeNull("listeners").AsReadOnly();
        }

        public void Recieve(IEnumerable<IEvent> events)
        {
            foreach (var linstener in _listeners)
            {
                linstener.Recieve(events);//TODO: as read only?
            }
        }
    }
}