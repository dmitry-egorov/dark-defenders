using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public class EntityAdapter<TEntity>
    {
        private readonly IContainer<TEntity> _rootContainer;
        private readonly IEventsProcessor _processor;

        public EntityAdapter(IContainer<TEntity> rootContainer, IEventsProcessor processor)
        {
            _rootContainer = rootContainer;
            _processor = processor;
        }

        public EntityAdapter<T> Commit<T>(Func<TEntity, ICreation<T>> command)
        {
            ICreation<T> creation = null;

            Commit(x => (IEnumerable<IEvent>)(creation = command(x)));

            return new EntityAdapter<T>(creation, _processor);
        }

        public void Commit(Func<TEntity, IEnumerable<IEvent>> command)
        {
            var events = command(_rootContainer.Item);
            _processor.Process(events);
        }
    }
}