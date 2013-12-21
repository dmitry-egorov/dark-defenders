using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public class RootsToProcessorAdapter<T>
    {
        private readonly ICommandProcessor _commandProcessor;

        public RootsToProcessorAdapter(ICommandProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor;
        }

        public void Do(Func<T, IEnumerable<IEvent>> command)
        {
            _commandProcessor.ProcessAllImplementing(command);
        }

        public void DoAndCommit(Func<T, IEnumerable<IEvent>> command)
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