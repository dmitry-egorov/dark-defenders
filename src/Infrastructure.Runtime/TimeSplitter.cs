using System;

namespace Infrastructure.Runtime
{
    public class TimeSplitter
    {
        private readonly TimeSpan _minGameElapsed;

        public TimeSplitter(TimeSpan minGameElapsed)
        {
            _minGameElapsed = minGameElapsed;
        }

        public void Split(TimeSpan elapsedTotal, Action<TimeSpan> action)
        {
            var timeStep = _minGameElapsed;

            while (elapsedTotal > timeStep)
            {
                elapsedTotal -= timeStep;

                action(timeStep);
            }

            if (elapsedTotal == TimeSpan.Zero)
            {
                return;
            }

            action(elapsedTotal);
        }
    }
}