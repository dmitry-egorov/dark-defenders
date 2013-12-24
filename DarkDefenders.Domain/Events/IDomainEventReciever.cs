using DarkDefenders.Domain.Players.Events;
using DarkDefenders.Domain.Projectiles.Events;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.RigidBodies.Events;
using DarkDefenders.Domain.Worlds;
using DarkDefenders.Domain.Worlds.Events;

namespace DarkDefenders.Domain.Events
{
    public interface IDomainEventReciever: IWorldEventsReciever, IPlayerEventsReciever, IRigidBodyEventsReciever, IProjectileEventsReciever
    {
        void Recieve(WorldCreated worldCreated);
        void Recieve(WorldDestroyed worldDestroyed);
        void Recieve(PlayerCreated playerCreated);
        void Recieve(PlayerDestroyed playerDestroyed);
        void Recieve(RigidBodyCreated rigidBodyCreated);
        void Recieve(RigidBodyDestroyed rigidBodyDestroyed);
        void Recieve(ProjectileCreated projectileCreated);
        void Recieve(ProjectileDestroyed projectileCreated);

    }
}