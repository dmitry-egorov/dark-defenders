using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public class RootsAdapter<TRoot>
    {
        private readonly IRepository<TRoot> _rootsRepository;
        private readonly IEventsProcessor _processor;

        public RootsAdapter(IRepository<TRoot> rootsRepository, IEventsProcessor processor)
        {
            _rootsRepository = rootsRepository;
            _processor = processor;
        }

        public void Commit(Func<TRoot, IEnumerable<IEvent>> command)
        {
            var events = _rootsRepository.ForAll(command);
            _processor.Process(events);
        }
    }
}