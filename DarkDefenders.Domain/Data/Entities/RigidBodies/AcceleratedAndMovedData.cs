using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Entities.RigidBodies;
using Infrastructure.Data;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.RigidBodies
{
    [ProtoContract]
    public class AcceleratedAndMovedData : EventDataBase
    {
        [ProtoMember(1)]
        public IdentityOf<RigidBody> RigidBodyId { get; private set; }

        [ProtoMember(2)]
        public VectorData NewPosition { get; private set; }

        [ProtoMember(3)]
        public MomentumData NewMomentum { get; private set; }

        private AcceleratedAndMovedData()//Protobuf
        {
        }

        public AcceleratedAndMovedData(IdentityOf<RigidBody> rigidBodyId, VectorData newPosition, MomentumData newMomentum)
        {
            RigidBodyId = rigidBodyId;
            NewPosition = newPosition;
            NewMomentum = newMomentum;
        }

        public override void Accept(IEventDataReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}