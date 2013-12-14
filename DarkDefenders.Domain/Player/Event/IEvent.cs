using Infrastructure.DDDEventSourcing.Domain;

namespace DarkDefenders.Domain.Player.Event
{
    public interface IEvent : IRootEvent<IEventReciever>
    {
    }
}