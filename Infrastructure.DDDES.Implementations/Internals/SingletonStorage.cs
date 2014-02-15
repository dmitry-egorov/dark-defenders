using System;
using JetBrains.Annotations;

namespace Infrastructure.DDDES.Implementations.Internals
{
    [UsedImplicitly]
    internal class SingletonStorage<TEntity> : IStorage<TEntity>
        where TEntity: class
    {
        private TEntity _entity;

        public void Store(TEntity entity)
        {
            if (_entity != null)
            {
                throw new InvalidOperationException("Entity is already set");
            }

            _entity = entity;
        }

        public void Remove(TEntity entity)
        {
            if (_entity != entity)
            {
                throw new InvalidOperationException("Invalid entity");
            }

            _entity = null;
        }
    }
}