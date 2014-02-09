using System.Runtime.Serialization;
using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Entities.RigidBodies;
using Infrastructure.Data;
using Infrastructure.DDDES;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.RigidBodies
{
    [ProtoContract]
    public class RigidBodyCreatedData : EventDataBase
    {
        [ProtoMember(1)]
        public IdentityOf<RigidBody> RigidBodyId { get; private set; }

        [ProtoMember(2)]
        public RigidBodyInitialProperties Properties { get; private set; }

        private RigidBodyCreatedData()//Protobuf
        {
        }

        public RigidBodyCreatedData(IdentityOf<RigidBody> rigidBodyId, RigidBodyInitialProperties properties)
        {
            RigidBodyId = rigidBodyId;
            Properties = properties;
        }

        public override void Accept(IEventDataReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}