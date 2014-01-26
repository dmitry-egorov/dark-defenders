using System;
using DarkDefenders.Dtos.Infrastructure;

namespace DarkDefenders.Dtos.Entities.Creatures
{
    public class FiredDto : EventDto<FiredDto>
    {
        public CreatureId CreatureId { get; private set; }
        public TimeSpan Time { get; private set; }

        public FiredDto(CreatureId creatureId, TimeSpan time)
        {
            CreatureId = creatureId;
            Time = time;
        }

        public override void Accept(IEventDtoReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}