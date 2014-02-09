using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Infrastructure
{
    public abstract class Created<TEntity> : Created<TEntity, IEventsReciever> 
        where TEntity : IEntity<TEntity>
    {
        protected Created(IStorage<TEntity> storage) : base(storage)
        {
        }
    }
}