using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public interface IBus: IObservable<IEvent>
    {
        void PublishTo<TRoot>(Identity id, Func<TRoot, IEnumerable<IEvent>> command);
        void PublishToAllOfType<T>(Func<T, IEnumerable<IEvent>> command);
    }
}