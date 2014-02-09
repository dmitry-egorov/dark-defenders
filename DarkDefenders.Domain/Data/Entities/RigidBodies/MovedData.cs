using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Entities.RigidBodies;
using Infrastructure.Data;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.RigidBodies
{
    [ProtoContract]
    public class MovedData : EventDataBase
    {
        [ProtoMember(1)]
        public IdentityOf<RigidBody> RigidBodyId { get; private set; }

        [ProtoMember(2)]
        public VectorData NewPosition { get; private set; }

        private MovedData()//Protobuf
        {
        }

        public MovedData(IdentityOf<RigidBody> rigidBodyId, VectorData newPosition)
        {
            RigidBodyId = rigidBodyId;
            NewPosition = newPosition;
        }

        public override void Accept(IEventDataReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}