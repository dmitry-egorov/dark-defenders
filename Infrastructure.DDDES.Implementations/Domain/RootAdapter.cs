using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public class RootAdapter<TRoot>
    {
        private readonly IContainer<TRoot> _rootContainer;
        private readonly IEventsProcessor _processor;

        public RootAdapter(IContainer<TRoot> rootContainer, IEventsProcessor processor)
        {
            _rootContainer = rootContainer;
            _processor = processor;
        }

        public void Commit(Func<TRoot, IEnumerable<IEvent>> command)
        {
            var events = command(_rootContainer.Item);
            _processor.Process(events);
        }
    }
}