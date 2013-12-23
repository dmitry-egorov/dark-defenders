using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public class RootsToProcessorAdapter<TRoot>
    {
        private readonly ICommandProcessor _commandProcessor;
        private readonly IRootCommandProcessor<TRoot> _rootProcessor;

        public RootsToProcessorAdapter(ICommandProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor;
            _rootProcessor = commandProcessor.GetProcessorFor<TRoot>();
        }

        public void Do(Func<TRoot, IEnumerable<IEvent>> command)
        {
            _rootProcessor.ProcessAll(command);
        }

        public void DoAndCommit(Func<TRoot, IEnumerable<IEvent>> command)
        {
            try
            {
                Do(command);
                _commandProcessor.Commit();
            }
            catch (Exception)
            {
                _commandProcessor.Rollback();
                throw;
            }
        }
    }
}