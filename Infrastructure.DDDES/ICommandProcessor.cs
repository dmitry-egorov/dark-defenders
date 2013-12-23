using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public interface ICommandProcessor : IUnitOfWork
    {
        void Process<TRoot>(Identity id, Func<TRoot, IEnumerable<IEvent>> command);
        void ProcessAll<TRoot>(Func<TRoot, IEnumerable<IEvent>> command);
        void Create<TRootFactory>(Func<TRootFactory, IEnumerable<IEvent>> creation);


        IRootCommandProcessor<TRoot> GetProcessorFor<TRoot>();
    }
}