namespace Infrastructure.DDDES
{
    public interface IRootEvent<in TEventReciever>: IEvent
    {
        void ApplyTo(TEventReciever reciever);
    }
}