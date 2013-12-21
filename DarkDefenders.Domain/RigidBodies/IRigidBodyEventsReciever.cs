using DarkDefenders.Domain.RigidBodies.Events;

namespace DarkDefenders.Domain.RigidBodies
{
    public interface IRigidBodyEventsReciever
    {
        void Apply(RigidBodyCreated rigidBodyCreated);
        void Apply(Moved moved);
        void Apply(Accelerated accelerated);
        void Apply(ExternalForceChanged externalForceChanged);
    }
}