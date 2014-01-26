using Infrastructure.Util;

namespace DarkDefenders.Dtos.Infrastructure
{
    public abstract class EventDto<TEventDto> : SlowValueObject<TEventDto>, IEventDto
    {
        public abstract void Accept(IEventDtoReciever reciever);
    }
}