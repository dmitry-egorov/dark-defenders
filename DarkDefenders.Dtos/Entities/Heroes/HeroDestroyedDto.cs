using DarkDefenders.Dtos.Infrastructure;

namespace DarkDefenders.Dtos.Entities.Heroes
{
    public class HeroDestroyedDto : EventDto<HeroDestroyedDto>
    {
        public HeroId HeroId { get; private set; }

        public HeroDestroyedDto(HeroId heroId)
        {
            HeroId = heroId;
        }

        public override void Accept(IEventDtoReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}