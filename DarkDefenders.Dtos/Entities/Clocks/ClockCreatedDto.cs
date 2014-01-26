using DarkDefenders.Dtos.Infrastructure;

namespace DarkDefenders.Dtos.Entities.Clocks
{
    public class ClockCreatedDto : EventDto<ClockCreatedDto>
    {
        public ClockId ClockId { get; private set; }

        public ClockCreatedDto(ClockId clockId)
        {
            ClockId = clockId;
        }

        public override void Accept(IEventDtoReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}