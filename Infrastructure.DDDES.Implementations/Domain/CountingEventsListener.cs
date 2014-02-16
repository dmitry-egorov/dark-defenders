using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public class CountingEventsListener<TReciever> : IEventsListener<TReciever>
    {
        private long _totalCount;
        private long _lastCount;

        public int EventsSinceLastCall
        {
            get
            {
                var count = _totalCount - _lastCount;
                _lastCount = _totalCount;

                return (int)count;
            }
        }

        public void Recieve(IEnumerable<Action<TReciever>> entityEvents)
        {
            _totalCount += entityEvents.Count();
        }
    }
}