using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public class CountingEventsListener<TReciever> : IEventsListener<TReciever>
    {
        private long _totalCount;
        private long _lastCount;

        public long TotalCount { get { return _totalCount; } }

        public int EventsSinceLastCall
        {
            get
            {
                var count = TotalCount - _lastCount;
                _lastCount = TotalCount;

                return (int)count;
            }
        }

        public void Recieve(IEnumerable<IAcceptorOf<TReciever>> entityEvents)
        {
            _totalCount += entityEvents.Count();
        }
    }
}