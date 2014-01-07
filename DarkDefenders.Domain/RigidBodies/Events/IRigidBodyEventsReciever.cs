namespace DarkDefenders.Domain.RigidBodies.Events
{
    public interface IRigidBodyEventsReciever
    {
        void Recieve(Moved moved);
        void Recieve(Accelerated accelerated);
        void Recieve(ExternalForceChanged externalForceChanged);
        void Recieve(AcceleratedAndMoved acceleratedAndMoved);
    }
}