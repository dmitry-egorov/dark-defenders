using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public static class BusExtensions
    {
        public static void ProcessAllAndCommit<T>(this ICommandProcessor commandProcessor, Func<T, IEnumerable<IEvent>> command)
        {
            try
            {
                commandProcessor.ProcessAllImplementing(command);
                commandProcessor.Commit();
            }
            catch(Exception)
            {
                commandProcessor.Rollback();
                throw;
            }
        }
        public static void ProcessAndCommit<TRoot>(this ICommandProcessor commandProcessor,Identity id, Func<TRoot, IEnumerable<IEvent>> command)
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

        public static RootToProcessorAdapter<TRoot> CreateAndCommit<TRoot>(this ICommandProcessor commandProcessor,Identity id, Func<TRoot, IEnumerable<IEvent>> command)
        {
            try
            {
                var adapter = commandProcessor.Create(id, command);
                commandProcessor.Commit();
                return adapter;
            }
            catch (Exception)
            {
                commandProcessor.Rollback();
                throw;
            }
        }

        public static RootToProcessorAdapter<TRoot> Create<TRoot>(this ICommandProcessor commandProcessor, Identity id, Func<TRoot, IEnumerable<IEvent>> command) 
        {
            commandProcessor.Process(id, command);
            return new RootToProcessorAdapter<TRoot>(commandProcessor, id);
        }
    }
}