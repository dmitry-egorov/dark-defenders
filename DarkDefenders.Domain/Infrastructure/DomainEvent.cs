using DarkDefenders.Dtos.Infrastructure;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Infrastructure
{
    public abstract class DomainEvent<TRoot, TId> : Event<TRoot, TId, IEventDto>
        where TRoot : IEntity<TId>
    {
        protected DomainEvent(TRoot root)
            : base(root)
        {
        }
    }
}