using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public class EntityAdapter<TEntity>
    {
        private readonly TEntity _entity;
        private readonly IEventsProcessor _processor;

        public EntityAdapter(TEntity entity, IEventsProcessor processor)
        {
            _entity = entity;
            _processor = processor;
        }

        public void Commit(Func<TEntity, IEnumerable<IEvent>> command)
        {
            var events = command(_entity);
            _processor.Process(events);
        }
    }
}