using DarkDefenders.Domain.RigidBodies;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Creatures
{
    public class CreatureProperties: ValueObject<CreatureProperties>
    {
        public double MovementForce { get; private set; }
        public double JumpMomentum { get; private set; }
        public RigidBodyProperties RigidBodyProperties { get; private set; }

        public CreatureProperties(double movementForce, double jumpMomentum, RigidBodyProperties rigidBodyProperties)
        {
            MovementForce = movementForce;
            JumpMomentum = jumpMomentum;
            RigidBodyProperties = rigidBodyProperties;
        }

        protected override string ToStringInternal()
        {
            return "{0}, {1}, {2}".FormatWith(MovementForce, JumpMomentum, RigidBodyProperties);
        }

        protected override bool EqualsInternal(CreatureProperties other)
        {
            return MovementForce.Equals(other.MovementForce)
                && JumpMomentum.Equals(other.JumpMomentum)
                && RigidBodyProperties.Equals(other.RigidBodyProperties);
        }

        protected override int GetHashCodeInternal()
        {
            var hashCode = MovementForce.GetHashCode();
            hashCode = (hashCode * 397) ^ JumpMomentum.GetHashCode();
            hashCode = (hashCode * 397) ^ RigidBodyProperties.GetHashCode();
            return hashCode;
        }
    }
}