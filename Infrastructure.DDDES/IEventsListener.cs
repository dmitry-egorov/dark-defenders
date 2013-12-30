namespace Infrastructure.DDDES
{
    public interface IEventsListener<in TDomainEvent>
    {
        void Recieve(TDomainEvent domainEvent);
    }
}