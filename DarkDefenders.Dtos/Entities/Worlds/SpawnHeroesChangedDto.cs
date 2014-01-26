using DarkDefenders.Dtos.Infrastructure;

namespace DarkDefenders.Dtos.Entities.Worlds
{
    public class SpawnHeroesChangedDto : EventDto<SpawnHeroesChangedDto>
    {
        public WorldId WorldId { get; private set; }
        public bool Enabled { get; private set; }

        public SpawnHeroesChangedDto(WorldId worldId, bool enabled)
        {
            WorldId = worldId;
            Enabled = enabled;
        }

        public override void Accept(IEventDtoReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}