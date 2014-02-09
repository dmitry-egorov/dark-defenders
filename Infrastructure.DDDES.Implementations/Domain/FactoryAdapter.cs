using System;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public class FactoryAdapter<TEntity, TFactory>
    {
        private readonly TFactory _factory;
        private readonly IEventsProcessor _processor;

        public FactoryAdapter(TFactory factory, IEventsProcessor processor)
        {
            _factory = factory;
            _processor = processor;
        }

        public EntityAdapter<TEntity> Commit(Func<TFactory, ICreation<TEntity>> command)
        {
            var creation = command(_factory);

            _processor.Process(creation);

            return new EntityAdapter<TEntity>(creation, _processor);
        }
    }
}