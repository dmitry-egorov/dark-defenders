using DarkDefenders.Dtos.Entities.Creatures;
using DarkDefenders.Dtos.Infrastructure;

namespace DarkDefenders.Dtos.Entities.Heroes
{
    public class HeroCreatedDto : EventDto<HeroCreatedDto>
    {
        public HeroId HeroId { get; private set; }
        public CreatureId CreatureId { get; private set; }

        public HeroCreatedDto(HeroId heroId, CreatureId creatureId)
        {
            HeroId = heroId;
            CreatureId = creatureId;
        }

        public override void Accept(IEventDtoReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}