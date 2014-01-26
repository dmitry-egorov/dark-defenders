using DarkDefenders.Dtos.Infrastructure;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Infrastructure
{
    public abstract class Destroyed<TRoot, TId> : Destroyed<TRoot, TId, IEventDto>
        where TRoot : IEntity<TId>
    {
        protected Destroyed(TRoot root, IStorage<TRoot> storage) : base(root, storage)
        {
        }
    }
}