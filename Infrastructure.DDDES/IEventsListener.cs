using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public interface IEventsListener<out TReciever>
    {
        void Recieve(IEnumerable<Action<TReciever>> entityEvents);
    }
}