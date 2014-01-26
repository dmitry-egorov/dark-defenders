using System.Collections.Generic;
using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations
{
    public class EventsProcessor<TEventDto> : IEventsProcessor
    {
        private readonly IEventsListener<TEventDto> _listener;

        public EventsProcessor(IEventsListener<TEventDto> listener)
        {
            _listener = listener;
        }

        public void Process(IEnumerable<IEvent> events)
        {
            var readOnly = events.AsReadOnly();

            readOnly.ApplyAll();
            foreach (var e in readOnly)
            {
                _listener.Recieve((TEventDto) e.ToDto());
            }
        }
    }
}