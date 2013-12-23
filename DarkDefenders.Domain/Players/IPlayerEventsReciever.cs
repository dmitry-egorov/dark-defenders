using DarkDefenders.Domain.Players.Events;

namespace DarkDefenders.Domain.Players
{
    public interface IPlayerEventsReciever
    {
        void Recieve(MovementForceChanged movementForceChanged);
        void Recieve(ProjectileCreated projectileCreated);
        void Recieve(ProjectileHitSomething projectileHitSomething);
    }
}