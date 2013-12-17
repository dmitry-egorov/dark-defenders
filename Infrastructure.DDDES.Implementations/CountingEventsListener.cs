using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.DDDES.Implementations
{
    public class CountingEventsListener: IEventsLinstener
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

        public void Apply(IEnumerable<IEvent> events)
        {
            _totalCount = TotalCount + events.Count();
        }
    }
}