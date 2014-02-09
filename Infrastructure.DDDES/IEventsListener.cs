namespace Infrastructure.DDDES
{
    public interface IEventsListener<in TEventData>
    {
        void Recieve(TEventData entityEvent);
    }
}