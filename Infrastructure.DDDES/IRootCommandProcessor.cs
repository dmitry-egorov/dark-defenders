using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public interface IRootCommandProcessor<out TRoot>
    {
        void Process(Identity id, Func<TRoot, IEnumerable<IEvent>> command);
        void ProcessAll(Func<TRoot, IEnumerable<IEvent>> command);
    }
}