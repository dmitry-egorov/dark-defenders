using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.Worlds;
using Infrastructure.Data;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.Worlds
{
    [ProtoContract]
    public class PlayerAvatarSpawnedData : EventDataBase
    {
        [ProtoMember(1)]
        public IdentityOf<World> WorldId { get; private set; }

        [ProtoMember(2)]
        public IdentityOf<Creature> CreatureId { get; private set; }

        private PlayerAvatarSpawnedData()//Protobuf
        {
        }

        public PlayerAvatarSpawnedData(IdentityOf<World> worldId, IdentityOf<Creature> creatureId)
        {
            WorldId = worldId;
            CreatureId = creatureId;
        }

        public override void Accept(IEventDataReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}