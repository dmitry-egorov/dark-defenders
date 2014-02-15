using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public class EntitiesAdapter<TEntity>
    {
        private readonly IReadOnlyCollection<TEntity> _entitiesRepository;
        private readonly IEventsProcessor _processor;

        public EntitiesAdapter(IReadOnlyCollection<TEntity> entitiesRepository, IEventsProcessor processor)
        {
            _entitiesRepository = entitiesRepository;
            _processor = processor;
        }

        public void Commit(Func<TEntity, IEnumerable<IEvent>> command)
        {
            var events = _entitiesRepository.ForAll(command);
            _processor.Process(events);
        }
    }
}