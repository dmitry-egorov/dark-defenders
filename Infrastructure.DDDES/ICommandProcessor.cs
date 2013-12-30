using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public interface ICommandProcessor<in TDomainEvent> : IUnitOfWork
    {
        void Process<TRoot>(Identity id, Func<TRoot, IEnumerable<TDomainEvent>> command);
        void ProcessAndCommit<TRoot>(Identity id, Func<TRoot, IEnumerable<TDomainEvent>> command);
        void ProcessAll<TRoot>(Func<TRoot, IEnumerable<TDomainEvent>> command);
        void ProcessAllAndCommit<TRoot>(Func<TRoot, IEnumerable<TDomainEvent>> command);
        void Create<TRootFactory>(Func<TRootFactory, IEnumerable<TDomainEvent>> creation);
        void CreateAndCommit<TRootFactory>(Func<TRootFactory, IEnumerable<TDomainEvent>> creation);

        IAllRootsAdapter<TRoot, TDomainEvent> CreateRootsAdapter<TRoot>();
        IRootAdapter<TRoot, TDomainEvent> CreateRootAdapter<TRoot>(Identity id);
    }
}