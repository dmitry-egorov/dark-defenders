using Infrastructure.Util;

namespace DarkDefenders.Game.Model.EntityProperties
{
    public class CreatureProperties: SlowValueObject
    {
        public double MovementForce { get; private set; }
        public double JumpMomentum { get; private set; }

        public CreatureProperties(double movementForce, double jumpMomentum)
        {
            MovementForce = movementForce;
            JumpMomentum = jumpMomentum;
        }
    }
}