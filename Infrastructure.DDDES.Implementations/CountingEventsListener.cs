namespace Infrastructure.DDDES.Implementations
{
    public class CountingEventsListener<TDomainEvent> : IEventsListener<TDomainEvent>
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

        public void Recieve(TDomainEvent entityEvent)
        {
            _totalCount += 1;
        }
    }
}