using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public interface ICommandProcessor : IUnitOfWork
    {
        T Request<T, TRoot>(Identity id, Func<TRoot, T> request);

        void Process<TRoot>(Identity id, Func<TRoot, IEnumerable<IEvent>> command);

        void ProcessAllImplementing<T>(Func<T, IEnumerable<IEvent>> command);
    }
}