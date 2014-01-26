namespace Infrastructure.DDDES
{
    public interface IEventsListener<in TEventDto>
    {
        void Recieve(TEventDto entityEvent);
    }
}