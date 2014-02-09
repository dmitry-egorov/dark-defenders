using System;

namespace Infrastructure.Runtime
{
    public class PerformanceCounter
    {
        private readonly TimeSpan _updatePeriod;
        private long _totalTicksCount;
        private int _ticksCount;
        private double _currentAverage;
        private readonly PeriodicExecutor _periodicExecutor;

        public PerformanceCounter() : this(TimeSpan.FromSeconds(1))
        {
            
        }

        public PerformanceCounter(TimeSpan updatePeriod)
        {
            _updatePeriod = updatePeriod;
            _periodicExecutor = new PeriodicExecutor(updatePeriod);
        }

        public bool Tick(int count, TimeSpan elapsed, out double ticksPerPeriod)
        {
            _totalTicksCount += count;
            _ticksCount += count;

            var changed = false;

            _periodicExecutor.Tick(elapsed, () =>
            {
                _currentAverage = _ticksCount / _updatePeriod.TotalSeconds;
                _ticksCount = 0;
                changed = true;
            });

            ticksPerPeriod = _currentAverage;
            return changed;
        }

        public void Tick(TimeSpan elapsed, Action<double, long> averageCountPerSecondChanged)
        {
            Tick(1, elapsed, averageCountPerSecondChanged);
        }

        public void Tick(int count, TimeSpan elapsed, Action<double, long> averageCountPerSecondChanged)
        {
            double average;
            if (Tick(count, elapsed, out average))
            {
                averageCountPerSecondChanged(average, _totalTicksCount);
            }
        }
    }
}