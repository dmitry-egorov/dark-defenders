using System.Collections.Generic;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public interface IRootsStorage<in TRootId, out TRoot, in TEvent> : IRepository<TRootId, TRoot>
    {
        void Apply(IEnumerable<TEvent> events);
    }
}