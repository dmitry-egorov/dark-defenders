using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public static class CommandProcessorExtensions
    {
        public static void ProcessAllAndCommit<T>(this ICommandProcessor commandProcessor, Func<T, IEnumerable<IEvent>> command)
        {
            try
            {
                commandProcessor.ProcessAll(command);
                commandProcessor.Commit();
            }
            catch(Exception)
            {
                commandProcessor.Rollback();
                throw;
            }
        }
        public static void ProcessAndCommit<TRoot>(this ICommandProcessor commandProcessor, Identity id, Func<TRoot, IEnumerable<IEvent>> command)
        {
            try
            {
                commandProcessor.Process(id, command);
                commandProcessor.Commit();
            }
            catch (Exception)
            {
                commandProcessor.Rollback();
                throw;
            }
        }

        public static void CreateAndCommit<TRootFactory>(this ICommandProcessor commandProcessor, Func<TRootFactory, IEnumerable<IEvent>> command)
        {
            try
            {
                commandProcessor.Create(command);
                commandProcessor.Commit();
            }
            catch (Exception)
            {
                commandProcessor.Rollback();
                throw;
            }
        }

        public static RootsToProcessorAdapter<TInterface> Create<TInterface>(this ICommandProcessor commandProcessor)
        {
            return new RootsToProcessorAdapter<TInterface>(commandProcessor);
        }
    }
}