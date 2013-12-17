using System;

namespace DarkDefenders.Console
{
    public class PerformanceCounter
    {
        private readonly TimeSpan _updatePeriod;

        private int _ticksCount;
        private TimeSpan _totalElapsed;
        private double _currentAverage;

        public PerformanceCounter() : this(TimeSpan.FromSeconds(1))
        {
            
        }

        public PerformanceCounter(TimeSpan updatePeriod)
        {
            _updatePeriod = updatePeriod;
        }

        public bool Tick(int count, TimeSpan elapsed, out double ticksPerPeriod)
        {
            var changed = _totalElapsed > _updatePeriod;
            if (changed)
            {
                _currentAverage = _ticksCount / _updatePeriod.TotalSeconds;

                _totalElapsed -= _updatePeriod;
                _ticksCount = 0;
            }
            else
            {
                _ticksCount += count;
                _totalElapsed += elapsed;
            }

            ticksPerPeriod = _currentAverage;
            return changed;
        }

        public void Tick(TimeSpan elapsed, Action<double> averageCountPerSecondChanged)
        {
            Tick(1, elapsed, averageCountPerSecondChanged);
        }

        public void Tick(int count, TimeSpan elapsed, Action<double> averageCountPerSecondChanged)
        {
            double average;
            if (Tick(count, elapsed, out average))
            {
                averageCountPerSecondChanged(average);
            }
        }

        public double Tick(int count, TimeSpan elapsed)
        {
            double fps;
            Tick(count, elapsed, out fps);
            return fps;
        }
    }
}