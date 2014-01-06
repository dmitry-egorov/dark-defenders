using DarkDefenders.Domain.Clocks;
using DarkDefenders.Domain.Clocks.Events;
using DarkDefenders.Domain.Creatures.Events;
using DarkDefenders.Domain.Projectiles.Events;
using DarkDefenders.Domain.RigidBodies.Events;
using DarkDefenders.Domain.Worlds.Events;

namespace DarkDefenders.Domain.Events
{
    public interface IDomainEventReciever: IWorldEventsReciever, ICreatureEventsReciever, IRigidBodyEventsReciever, IProjectileEventsReciever, IClockEventsReciever
    {
        void Recieve(ClockCreated clockCreated);
        void Recieve(ClockDestroyed clockDestroyed);
        void Recieve(WorldCreated worldCreated);
        void Recieve(WorldDestroyed worldDestroyed);
        void Recieve(CreatureCreated creatureCreated);
        void Recieve(CreatureDestroyed creatureDestroyed);
        void Recieve(RigidBodyCreated rigidBodyCreated);
        void Recieve(RigidBodyDestroyed rigidBodyDestroyed);
        void Recieve(ProjectileCreated projectileCreated);
        void Recieve(ProjectileDestroyed projectileCreated);
    }
}