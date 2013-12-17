using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public class RootToProcessorAdapter<TRoot>
    {
        private readonly ICommandProcessor _commandProcessor;
        private readonly Identity _id;

        public RootToProcessorAdapter(ICommandProcessor commandProcessor, Identity id)
        {
            _commandProcessor = commandProcessor;
            _id = id;
        }

        public void Do(Func<TRoot, IEnumerable<IEvent>> command)
        {
            _commandProcessor.Process(_id, command);
        }

        public T Request<T>(Func<TRoot, T> func)
        {
            return _commandProcessor.Request(_id, func);
        }
    }
}
