using System;

namespace DarkDefenders.Console
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

            if (_totalElapsed < _executionPeriod)
            {
                return;
            }

            _totalElapsed -= _executionPeriod;
            action();
        }
    }
}