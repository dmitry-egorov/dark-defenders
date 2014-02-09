using System.Collections.Generic;

namespace Infrastructure.DDDES.Implementations
{
    public class EventsProcessor<TEventData> : IEventsProcessor
    {
        private readonly IEventsListener<TEventData> _listener;

        public EventsProcessor(IEventsListener<TEventData> listener)
        {
            _listener = listener;
        }

        public void Process(IEnumerable<IEvent> events)
        {
            foreach (var e in events)
            {
                e.Apply();
                _listener.Recieve((TEventData) e.GetData());
            }
        }
    }
}