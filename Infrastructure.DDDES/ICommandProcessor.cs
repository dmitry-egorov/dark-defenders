using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public interface ICommandProcessor<in TDomainEvent> : IUnitOfWork
    {
        void Process<TRoot>(Identity id, Func<TRoot, IEnumerable<TDomainEvent>> command);
        void Commit<TRoot>(Identity id, Func<TRoot, IEnumerable<TDomainEvent>> command);
        void ProcessAll<TRoot>(Func<TRoot, IEnumerable<TDomainEvent>> command);
        void CommitAll<TRoot>(Func<TRoot, IEnumerable<TDomainEvent>> command);
        void Create<TRootFactory>(Func<TRootFactory, IEnumerable<TDomainEvent>> creation);
        void CommitCreation<TRootFactory>(Func<TRootFactory, IEnumerable<TDomainEvent>> creation);

        IAllRootsAdapter<TRoot, TDomainEvent> CreateRootsAdapter<TRoot>();
        IRootAdapter<TRoot, TDomainEvent> CreateRootAdapter<TRoot>(Identity id);
    }
}