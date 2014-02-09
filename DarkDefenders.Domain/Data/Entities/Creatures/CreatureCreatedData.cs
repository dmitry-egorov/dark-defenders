using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.RigidBodies;
using Infrastructure.Data;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.Creatures
{
    [ProtoContract]
    public class CreatureCreatedData : EventDataBase
    {
        [ProtoMember(1)]
        public IdentityOf<Creature> CreatureId { get; private set; }
        [ProtoMember(2)]
        public IdentityOf<RigidBody> RigidBodyId { get; private set; }
        [ProtoMember(3)]
        public CreatureProperties Properties { get; private set; }

        private CreatureCreatedData()//Protobuf
        {
        }

        public CreatureCreatedData(IdentityOf<Creature> creatureId, IdentityOf<RigidBody> rigidBodyId, CreatureProperties properties)
        {
            CreatureId = creatureId;
            RigidBodyId = rigidBodyId;
            Properties = properties;
        }

        public override void Accept(IEventDataReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}