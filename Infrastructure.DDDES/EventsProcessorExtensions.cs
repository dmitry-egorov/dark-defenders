using System;
using System.Collections.Generic;
using Infrastructure.Util;

namespace Infrastructure.DDDES
{
    public static class EventsProcessorExtensions
    {
        public static void Process<TEntity>(this IEventsProcessor processor, IEnumerable<TEntity> entities, Func<TEntity, IEnumerable<IEvent>> command)
        {
            processor.Process(entities.ForAll(command));
        }

        public static void Process(this IEventsProcessor processor, params IEnumerable<IEvent>[] eventPacks)
        {
            var events = Concat.All(eventPacks);

            processor.Process(events);
        }
    }
}