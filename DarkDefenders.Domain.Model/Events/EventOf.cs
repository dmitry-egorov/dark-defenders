using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Model.Events
{
    internal abstract class EventOf<TEntity> : EventOf<TEntity, IEventsReciever>
        where TEntity : IEntity<TEntity>
    {
        protected EventOf(TEntity entity) : base(entity)
        {
        }
    }
}