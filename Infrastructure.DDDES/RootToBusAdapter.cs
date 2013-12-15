using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public class RootToBusAdapter<TRoot>
    {
        private readonly IBus _bus;
        private readonly Identity _id;

        public RootToBusAdapter(IBus bus, Identity id)
        {
            _bus = bus;
            _id = id;
        }

        public void Do(Func<TRoot, IEnumerable<IEvent>> command)
        {
            _bus.PublishTo(_id, command);
        }
    }
}
