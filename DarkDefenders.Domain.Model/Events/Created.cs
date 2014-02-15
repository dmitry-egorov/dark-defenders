using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Model.Events
{
    internal abstract class Created<TEntity> : Created<TEntity, IEventsReciever> 
        where TEntity : IEntity<TEntity>
    {
        protected Created(TEntity entity, IStorage<TEntity> storage) : base(entity, storage)
        {
        }
    }
}