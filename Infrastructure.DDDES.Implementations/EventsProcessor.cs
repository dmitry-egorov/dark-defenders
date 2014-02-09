using System.Collections.Generic;
using System.Linq;
using Infrastructure.Util;
using JetBrains.Annotations;

namespace Infrastructure.DDDES.Implementations
{
    [UsedImplicitly]
    public class EventsProcessor<TReciever> : IEventsProcessor
    {
        private readonly Queue<IAcceptorOf<TReciever>> _queue = new Queue<IAcceptorOf<TReciever>>();

        private readonly IEventsListener<TReciever> _listener;

        public EventsProcessor(IEventsListener<TReciever> listener)
        {
            _listener = listener;
        }

        public void Process(IEnumerable<IEvent> events)
        {
            foreach (var e in events)
            {
                e.Apply();
                _queue.Enqueue((IAcceptorOf<TReciever>) e);
            }
        }

        public void Broadcast()
        {
            var events = _queue.DequeueAll().ToList();

            _listener.Recieve(events);
        }
    }
}