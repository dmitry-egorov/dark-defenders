namespace Infrastructure.DDDES
{
    public interface IRootEvent<out TRootId, in TEventReciever>: IEvent<TRootId>
    {
        void ApplyTo(TEventReciever reciever);
    }
}