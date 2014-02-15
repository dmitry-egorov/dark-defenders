using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Events
{
    internal abstract class Destroyed<TEntity> : Destroyed<TEntity, IEventsReciever> 
        where TEntity : IEntity<TEntity>
    {
        protected Destroyed(TEntity entity, IStorage<TEntity> storage) : base(entity, storage)
        {
        }
    }
}