using DarkDefenders.Domain.Worlds;
using Infrastructure.Math;
using Infrastructure.Math.Physics;

namespace DarkDefenders.Domain.Other
{
    public class Cooldown
    {
        private readonly World _world;
        private readonly Seconds _cooldownDelay;

        private Seconds _activationTime = Seconds.Zero;

        public Cooldown(World world, Seconds cooldownDelay)
        {
            _world = world;
            _cooldownDelay = cooldownDelay;
        }

        public void Activate(Seconds activationTime)
        {
            _activationTime = activationTime;
        }

        public bool IsInEffect()
        {
            return _world.GetCurrentTime() - _activationTime < _cooldownDelay;
        }
    }
}