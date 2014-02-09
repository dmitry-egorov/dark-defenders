using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Entities.RigidBodies;
using Infrastructure.Data;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.RigidBodies
{
    [ProtoContract]
    public class AcceleratedData : EventDataBase
    {
        [ProtoMember(1)]
        public IdentityOf<RigidBody> RigidBodyId { get; private set; }

        [ProtoMember(2)]
        public MomentumData NewMomentum { get; private set; }

        private AcceleratedData()//Protobuf
        {
        }

        public AcceleratedData(IdentityOf<RigidBody> rigidBodyId, MomentumData newMomentum)
        {
            RigidBodyId = rigidBodyId;
            NewMomentum = newMomentum;
        }

        public override void Accept(IEventDataReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}