using System.Runtime.Serialization;
using Infrastructure.Data;
using Infrastructure.Util;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.RigidBodies
{
    [ProtoContract]
    public class RigidBodyInitialProperties : SlowValueObject
    {
        [ProtoMember(1)]
        public MomentumData InitialMomentum { get; private set; }
        [ProtoMember(2)]
        public VectorData Position { get; private set; }
        [ProtoMember(3)]
        public RigidBodyProperties Properties { get; private set; }

        public RigidBodyInitialProperties()//Protobuf
        {
        }

        public RigidBodyInitialProperties(MomentumData initialMomentum, VectorData position, RigidBodyProperties properties)
        {
            InitialMomentum = initialMomentum;
            Position = position;
            Properties = properties;
        }
    }
}