using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public static class DelegatingEventsListener
    {
        public static DelegatingEventsListener<TReciever> Create<TReciever>(TReciever reciever)
        {
            return new DelegatingEventsListener<TReciever>(reciever);
        }
    }

    public class DelegatingEventsListener<TReciever> : IEventsListener<TReciever>
    {
        private readonly TReciever _reciever;

        public DelegatingEventsListener(TReciever reciever)
        {
            _reciever = reciever;
        }

        public void Recieve(IEnumerable<Action<TReciever>> entityEvents)
        {
            foreach (var entityEvent in entityEvents)
            {
                entityEvent(_reciever);
            }
        }
    }
}