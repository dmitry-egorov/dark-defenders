using DarkDefenders.Domain.RigidBodies.Events;

namespace DarkDefenders.Domain.RigidBodies
{
    public interface IRigidBodyEventsReciever
    {
        void Recieve(Moved moved);
        void Recieve(Accelerated accelerated);
        void Recieve(ExternalForceChanged externalForceChanged);
    }
}