using System.Runtime.Serialization;
using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Entities.RigidBodies;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.Physics;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.RigidBodies
{
    [ProtoContract]
    public class ExternalForceChangedData : EventDataBase
    {
        [ProtoMember(1)]
        public IdentityOf<RigidBody> RigidBodyId { get; private set; }

        [ProtoMember(2)]
        public Force ExternalForce { get; private set; }

        private ExternalForceChangedData()//Protobuf
        {
        }

        public ExternalForceChangedData(IdentityOf<RigidBody> rigidBodyId, Force externalForce)
        {
            RigidBodyId = rigidBodyId;
            ExternalForce = externalForce;
        }

        public override void Accept(IEventDataReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}