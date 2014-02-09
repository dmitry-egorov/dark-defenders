using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Events
{
    internal abstract class Created<TEntity> : Created<TEntity, IEventsReciever> 
        where TEntity : IEntity<TEntity>
    {
        protected Created(IStorage<TEntity> storage) : base(storage)
        {
        }
    }
}