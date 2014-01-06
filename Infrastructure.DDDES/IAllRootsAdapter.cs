using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public interface IAllRootsAdapter<out TRoot, in TDomainEvent>
    {
        void Do(Func<TRoot, IEnumerable<TDomainEvent>> command);
        void Commit(Func<TRoot, IEnumerable<TDomainEvent>> command);
    }
}