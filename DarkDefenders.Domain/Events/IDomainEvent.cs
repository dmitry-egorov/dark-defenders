namespace DarkDefenders.Domain.Events
{
    public interface IDomainEvent
    {
        void Accept(IDomainEventReciever reciever);
    }
}