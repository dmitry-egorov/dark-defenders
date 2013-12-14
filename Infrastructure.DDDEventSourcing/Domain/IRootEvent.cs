namespace Infrastructure.DDDEventSourcing.Domain
{
    public interface IRootEvent<in TEventReciever>: IEventMarker
    {
        void ApplyTo(TEventReciever reciever);
    }
}