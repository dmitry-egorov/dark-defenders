using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public class RootToProcessorAdapter<TRoot>
    {
        private readonly IRootCommandProcessor<TRoot> _commandProcessor;
        private readonly Identity _id;

        public RootToProcessorAdapter(Identity id, ICommandProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor.GetProcessorFor<TRoot>();
            _id = id;
        }

        public void Do(Func<TRoot, IEnumerable<IEvent>> command)
        {
            _commandProcessor.Process(_id, command);
        }
    }
}
