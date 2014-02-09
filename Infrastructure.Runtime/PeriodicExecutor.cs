using System;

namespace Infrastructure.Runtime
{
    public class PeriodicExecutor
    {
        private TimeSpan _totalElapsed;
        private readonly TimeSpan _executionPeriod;

        public PeriodicExecutor(TimeSpan executionPeriod)
        {
            _executionPeriod = executionPeriod;
        }

        public void Tick(TimeSpan elapsed, Action action)
        {
            _totalElapsed += elapsed;

            while (_totalElapsed >= _executionPeriod)
            {
                _totalElapsed -= _executionPeriod;
                action();
            }
        }
    }
}