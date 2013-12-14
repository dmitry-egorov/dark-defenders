using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public interface ICommandProcessor
    {
        IEnumerable<IEvent> Process<TRoot>(Identity id, Func<TRoot, IEnumerable<IEvent>> command);

        IEnumerable<IEvent> ProcessAllImplementing<T>(Func<T, IEnumerable<IEvent>> command);
    }
}