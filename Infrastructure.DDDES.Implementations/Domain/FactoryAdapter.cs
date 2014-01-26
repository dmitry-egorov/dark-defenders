using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public class FactoryAdapter<TFactory>
    {
        private readonly TFactory _factory;
        private readonly IEventsProcessor _processor;

        public FactoryAdapter(TFactory factory, IEventsProcessor processor)
        {
            _factory = factory;
            _processor = processor;
        }

        public void Commit(Func<TFactory, IEnumerable<IEvent>> command)
        {
            var events = command(_factory);
            _processor.Process(events);
        }
    }
}