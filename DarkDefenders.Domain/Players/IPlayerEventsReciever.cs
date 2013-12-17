using DarkDefenders.Domain.Players.Events;

namespace DarkDefenders.Domain.Players
{
    public interface IPlayerEventsReciever
    {
        void Apply(PlayerCreated playerCreated);
        void Apply(MovementForceChanged movementForceChanged);
        void Apply(PlayerMoved playerMoved);
        void Apply(PlayerAccelerated playerAccelerated);
    }
}