using DarkDefenders.Dtos.Infrastructure;

namespace DarkDefenders.Dtos.Entities.Worlds
{
    public class WorldCreatedDto : EventDto<WorldCreatedDto>
    {
        public WorldId Id { get; private set; }
        public WorldProperties WorldProperties { get; private set; }

        public WorldCreatedDto(WorldId id, WorldProperties worldProperties)
        {
            Id = id;
            WorldProperties = worldProperties;
        }

        public override void Accept(IEventDtoReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}