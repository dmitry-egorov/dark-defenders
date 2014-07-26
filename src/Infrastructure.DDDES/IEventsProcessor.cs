namespace Infrastructure.DDDES
{
    public interface IEventsProcessor
    {
        void Publish(IEvent e);
        void Process();
    }
}