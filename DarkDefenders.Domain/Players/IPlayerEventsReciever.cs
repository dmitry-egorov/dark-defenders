using DarkDefenders.Domain.Players.Events;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Players
{
    public interface IPlayerEventsReciever
    {
        void Apply(PlayerCreated playerCreated);
        void Apply(MovementForceChanged movementForceChanged);
    }
}