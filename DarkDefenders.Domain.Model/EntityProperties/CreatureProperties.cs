using Infrastructure.Util;

namespace DarkDefenders.Domain.Model.EntityProperties
{
    public class CreatureProperties: SlowValueObject
    {
        public float MovementForce { get; private set; }
        public float JumpMomentum { get; private set; }

        public CreatureProperties(float movementForce, float jumpMomentum)
        {
            MovementForce = movementForce;
            JumpMomentum = jumpMomentum;
        }
    }
}