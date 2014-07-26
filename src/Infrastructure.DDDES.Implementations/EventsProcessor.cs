using System.Collections.Generic;
using Infrastructure.Util;
using JetBrains.Annotations;

namespace Infrastructure.DDDES.Implementations
{
    [UsedImplicitly]
    public class EventsProcessor : IEventsProcessor
    {
        private readonly Queue<IEvent> _queue = new Queue<IEvent>();

        public void Publish(IEvent e)
        {
            _queue.Enqueue(e);
        }

        public void Process()
        {
            var all = _queue.DequeueAll();

            foreach (var @event in all)
            {
                @event.Apply();
            }
        }
    }
}