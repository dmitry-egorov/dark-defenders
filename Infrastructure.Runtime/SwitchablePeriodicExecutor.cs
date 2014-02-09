using System;

namespace Infrastructure.Runtime
{
    public class SwitchablePeriodicExecutor
    {
        private readonly PeriodicExecutor _executor;
        private readonly OnOffSwitch _testHeroSpawningSwitch;

        public SwitchablePeriodicExecutor(TimeSpan period, bool isOn)
        {
            _executor = new PeriodicExecutor(period);
            _testHeroSpawningSwitch = new OnOffSwitch(isOn);
        }

        public void Tick(TimeSpan elapsed, Action action)
        {
            _executor.Tick(elapsed, () => _testHeroSpawningSwitch.WhenOn(action));
        }

        public void State(bool isPressed)
        {
            _testHeroSpawningSwitch.State(isPressed);
        }
    }
}