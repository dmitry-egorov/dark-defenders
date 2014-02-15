using DarkDefenders.Domain.Entities.RigidBodies;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Entities.Creatures
{
    public class CreatureProperties: SlowValueObject
    {
        public float MovementForce { get; private set; }
        public float JumpMomentum { get; private set; }
        public RigidBodyProperties RigidBodyProperties { get; private set; }

        public CreatureProperties(float movementForce, float jumpMomentum, RigidBodyProperties rigidBodyProperties)
        {
            MovementForce = movementForce;
            JumpMomentum = jumpMomentum;
            RigidBodyProperties = rigidBodyProperties.ShouldNotBeNull("rigidBodyProperties");
        }
    }
}