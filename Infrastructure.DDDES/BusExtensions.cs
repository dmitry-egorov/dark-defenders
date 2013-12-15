using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public static class BusExtensions
    {
        public static RootToBusAdapter<TRoot> Create<TRoot>(this IBus bus, Identity id, Func<TRoot, IEnumerable<IEvent>> command) 
        {
            bus.PublishTo(id, command);

            return new RootToBusAdapter<TRoot>(bus, id);
        }
    }
}