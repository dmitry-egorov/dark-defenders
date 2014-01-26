using DarkDefenders.Dtos.Entities.RigidBodies;
using Infrastructure.Util;

namespace DarkDefenders.Dtos.Entities.Creatures
{
    public class CreatureProperties: SlowValueObject<CreatureProperties>
    {
        public double MovementForce { get; private set; }
        public double JumpMomentum { get; private set; }
        public RigidBodyProperties RigidBodyProperties { get; private set; }

        public CreatureProperties(double movementForce, double jumpMomentum, RigidBodyProperties rigidBodyProperties)
        {
            MovementForce = movementForce;
            JumpMomentum = jumpMomentum;
            RigidBodyProperties = rigidBodyProperties.ShouldNotBeNull("rigidBodyProperties");
        }
    }
}