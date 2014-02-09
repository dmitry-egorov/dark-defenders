using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Entities.RigidBodies;
using Infrastructure.Data;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.RigidBodies
{
    [ProtoContract]
    public class RigidBodyDestroyedData : EventDataBase
    {
        [ProtoMember(1)]
        public IdentityOf<RigidBody> RigidBodyId { get; private set; }

        private RigidBodyDestroyedData()//Protobuf
        {
        }

        public RigidBodyDestroyedData(IdentityOf<RigidBody> rigidBodyId)
        {
            RigidBodyId = rigidBodyId;
        }

        public override void Accept(IEventDataReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}