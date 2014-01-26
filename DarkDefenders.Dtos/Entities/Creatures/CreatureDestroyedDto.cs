using DarkDefenders.Dtos.Infrastructure;

namespace DarkDefenders.Dtos.Entities.Creatures
{
    public class CreatureDestroyedDto : EventDto<CreatureDestroyedDto>
    {
        public CreatureId Id { get; private set; }

        public CreatureDestroyedDto(CreatureId id)
        {
            Id = id;
        }

        public override void Accept(IEventDtoReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}