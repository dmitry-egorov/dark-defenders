using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Events
{
    internal abstract class EventOf<TEntity> : EventOf<TEntity, IEventsReciever>
        where TEntity : IEntity<TEntity>
    {
        protected EventOf(TEntity entity) : base(entity)
        {
        }
    }
}