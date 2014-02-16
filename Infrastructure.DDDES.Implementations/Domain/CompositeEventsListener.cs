using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public static class CompositeEventsListener
    {
        public static CompositeEventsListener<TReciever> Create<TReciever>(params IEventsListener<TReciever>[] linsteners)
        {
            return new CompositeEventsListener<TReciever>(linsteners);
        }
    }

    public class CompositeEventsListener<TReciever> : IEventsListener<TReciever>
    {
        private readonly ReadOnlyCollection<IEventsListener<TReciever>> _listeners;

        public CompositeEventsListener(params IEventsListener<TReciever>[] listeners)
            : this(listeners.AsEnumerable())
        {
        }

        public CompositeEventsListener(IEnumerable<IEventsListener<TReciever>> linsteners)
        {
            _listeners = linsteners.ShouldNotBeNull("listeners").AsReadOnly();
        }

        public void Recieve(IEnumerable<Action<TReciever>> entityEvents)
        {
            var readOnly = entityEvents.AsReadOnly();
            foreach (var linstener in _listeners)
            {
                linstener.Recieve(readOnly);
            }
        }
    }
}