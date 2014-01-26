using DarkDefenders.Dtos.Infrastructure;
using DarkDefenders.Dtos.Other;

namespace DarkDefenders.Dtos.Entities.Creatures
{
    public class MovementChangedDto : EventDto<MovementChangedDto>
    {
        public CreatureId CreatureId { get; private set; }
        public Movement Movement { get; private set; }

        public MovementChangedDto(CreatureId creatureId, Movement movement)
        {
            CreatureId = creatureId;
            Movement = movement;
        }

        public override void Accept(IEventDtoReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}