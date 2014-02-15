using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public static class EventsProcessorExtensions
    {
        public static void Process<TEntity>(this IEventsProcessor processor, IEnumerable<TEntity> entities, Func<TEntity, IEnumerable<IEvent>> command)
        {
            processor.Process(entities.ForAll(command));
        }
        
    }
}