using DarkDefenders.Domain.Players;
using DarkDefenders.Domain.Players.Events;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.RigidBodies.Events;
using DarkDefenders.Domain.Worlds;
using DarkDefenders.Domain.Worlds.Events;

namespace DarkDefenders.Domain.Events
{
    public interface IDomainEventReciever: IWorldEventsReciever, IPlayerEventsReciever, IRigidBodyEventsReciever
    {
        void Recieve(WorldCreated worldCreated);
        void Recieve(WorldRemoved worldRemoved);
        void Recieve(PlayerCreated playerCreated);
        void Recieve(PlayerRemoved playerRemoved);
        void Recieve(RigidBodyCreated rigidBodyCreated);
        void Recieve(RigidBodyRemoved rigidBodyRemoved);
    }
}