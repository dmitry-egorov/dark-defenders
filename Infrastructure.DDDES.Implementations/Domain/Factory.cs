using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class Factory<TEntity> 
        where TEntity : class
    {
        private readonly IStorage<TEntity> _storage;

        protected Factory(IStorage<TEntity> storage)
        {
            _storage = storage;
        }

        protected ICreation<TEntity> GetCreation(Func<IStorage<TEntity>, IEnumerable<IEvent>> yieldFunc)
        {
            return For(_storage, yieldFunc);
        }

        private static ICreation<TEntity> For(IStorage<TEntity> storage, Func<IStorage<TEntity>, IEnumerable<IEvent>> yieldFunc)
        {
            var container = new Container<TEntity>();

            var compositeStorage = storage.ComposeWith(container);

            var events = yieldFunc(compositeStorage);

            return new Creation<TEntity>(events, container);
        }
    }
}