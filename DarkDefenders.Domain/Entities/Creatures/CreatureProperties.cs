using DarkDefenders.Domain.Data.Entities.RigidBodies;
using Infrastructure.Util;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.Creatures
{
    [ProtoContract]
    public class CreatureProperties: SlowValueObject
    {
        [ProtoMember(1)]
        public float MovementForce { get; private set; }
        [ProtoMember(2)]
        public float JumpMomentum { get; private set; }
        [ProtoMember(3)]
        public RigidBodyProperties RigidBodyProperties { get; private set; }

        public CreatureProperties()//Protobuf
        {
        }

        public CreatureProperties(float movementForce, float jumpMomentum, RigidBodyProperties rigidBodyProperties)
        {
            MovementForce = movementForce;
            JumpMomentum = jumpMomentum;
            RigidBodyProperties = rigidBodyProperties.ShouldNotBeNull("rigidBodyProperties");
        }
    }
}