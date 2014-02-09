using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Infrastructure
{
    public abstract class Destroyed<TEntity> : Destroyed<TEntity, IEventsReciever> 
        where TEntity : IEntity<TEntity>
    {
        protected Destroyed(TEntity root, IStorage<TEntity> storage) : base(root, storage)
        {
        }
    }
}