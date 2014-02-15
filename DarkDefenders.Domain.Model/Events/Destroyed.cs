using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Model.Events
{
    internal abstract class Destroyed<TEntity> : Destroyed<TEntity, IEventsReciever> 
        where TEntity : IEntity<TEntity>
    {
        protected Destroyed(TEntity entity, IStorage<TEntity> storage) : base(entity, storage)
        {
        }
    }
}