using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Infrastructure
{
    public abstract class EventOf<TEntity> : EventOf<TEntity, IEventsReciever> 
        where TEntity : IEntity<TEntity>
    {
        protected EventOf(TEntity root) : base(root)
        {
        }
    }
}