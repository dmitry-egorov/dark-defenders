using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Events
{
    public interface IDomainEvent: IEvent
    {
        void ApplyTo(IDomainEventsReciever reciever);
    }
}