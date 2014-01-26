using DarkDefenders.Dtos.Infrastructure;

namespace DarkDefenders.Dtos.Entities.Heroes
{
    public class StateChangedDto : EventDto<StateChangedDto>
    {
        public HeroId HeroId { get; private set; }
        public HeroStateDto State { get; private set; }

        public StateChangedDto(HeroId heroId, HeroStateDto state)
        {
            HeroId = heroId;
            State = state;
        }

        public override void Accept(IEventDtoReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}