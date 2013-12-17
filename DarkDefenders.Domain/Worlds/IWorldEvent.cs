using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Worlds
{
    public interface IWorldEvent : IRootEvent<WorldId, IWorldEventsReciever>
    {
    }
}