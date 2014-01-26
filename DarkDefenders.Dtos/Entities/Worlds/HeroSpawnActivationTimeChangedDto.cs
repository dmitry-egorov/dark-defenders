using System;
using DarkDefenders.Dtos.Infrastructure;

namespace DarkDefenders.Dtos.Entities.Worlds
{
    public class HeroSpawnActivationTimeChangedDto : EventDto<HeroSpawnActivationTimeChangedDto>
    {
        public WorldId WorldId { get; private set; }
        public TimeSpan Time { get; private set; }

        public HeroSpawnActivationTimeChangedDto(WorldId worldId, TimeSpan time)
        {
            WorldId = worldId;
            Time = time;
        }

        public override void Accept(IEventDtoReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}