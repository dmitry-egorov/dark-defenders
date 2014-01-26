using DarkDefenders.Dtos.Entities.Creatures;
using DarkDefenders.Dtos.Infrastructure;

namespace DarkDefenders.Dtos.Entities.Worlds
{
    public class PlayerAvatarSpawnedDto: EventDto<PlayerAvatarSpawnedDto>
    {
        public WorldId WorldId { get; private set; }
        public CreatureId CreatureId { get; private set; }

        public PlayerAvatarSpawnedDto(WorldId worldId, CreatureId creatureId)
        {
            WorldId = worldId;
            CreatureId = creatureId;
        }

        public override void Accept(IEventDtoReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}