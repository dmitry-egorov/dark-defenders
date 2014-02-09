using Infrastructure.Util;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.RigidBodies
{
    [ProtoContract]
    public class RigidBodyProperties : SlowValueObject
    {
        [ProtoMember(1)]
        public float BoundingBoxRadius { get; private set; }
        [ProtoMember(2)]
        public float Mass { get; private set; }
        [ProtoMember(3)]
        public float TopHorizontalMomentum { get; private set; }

        public RigidBodyProperties()//Protobuf
        {
        }

        public RigidBodyProperties(float boundingBoxRadius, float mass, float topHorizontalMomentum)
        {
            BoundingBoxRadius = boundingBoxRadius;
            Mass = mass;
            TopHorizontalMomentum = topHorizontalMomentum;
        }
    }
}