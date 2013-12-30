using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES.Implementations.Internal
{
    internal interface IRootCommandProcessor<out TRoot, in TDomainEvent>
    {
        void Process(Identity id, Func<TRoot, IEnumerable<TDomainEvent>> command);
        void ProcessAll(Func<TRoot, IEnumerable<TDomainEvent>> command);
    }
}