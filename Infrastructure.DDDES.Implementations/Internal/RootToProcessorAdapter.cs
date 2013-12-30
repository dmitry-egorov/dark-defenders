using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES.Implementations.Internal
{
    internal class RootToProcessorAdapter<TRoot, TDomainEvent> : IRootAdapter<TRoot, TDomainEvent> 
        where TDomainEvent : IEvent
    {
        private readonly IRootCommandProcessor<TRoot, TDomainEvent> _commandProcessor;
        private readonly Identity _id;

        public RootToProcessorAdapter(Identity id, IRootCommandProcessor<TRoot, TDomainEvent> commandProcessor)
        {
            _commandProcessor = commandProcessor;
            _id = id;
        }

        public void Do(Func<TRoot, IEnumerable<TDomainEvent>> command)
        {
            _commandProcessor.Process(_id, command);
        }
    }
}
