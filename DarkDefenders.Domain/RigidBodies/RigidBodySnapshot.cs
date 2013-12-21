using DarkDefenders.Domain.RigidBodies.Events;
using DarkDefenders.Domain.Worlds;
using Infrastructure.Math;

namespace DarkDefenders.Domain.RigidBodies
{
    public class RigidBodySnapshot : IRigidBodyEventsReciever
    {
        public WorldId WorldId { get; private set; }
        public Vector Position { get; private set; }
        public Vector Momentum { get; private set; }
        public Vector ExternalForce { get; private set; }
        
        public void Apply(RigidBodyCreated rigidBodyCreated)
        {
            WorldId = rigidBodyCreated.WorldId;
            Position = rigidBodyCreated.SpawnPosition;
            Momentum = Vector.Zero;
            ExternalForce = Vector.Zero;
        }

        public void Apply(Moved rigidBodyAccelerated)
        {
            Position = rigidBodyAccelerated.NewPosition;
        }

        public void Apply(Accelerated accelerated)
        {
            Momentum = accelerated.NewMomentum;
        }

        public void Apply(ExternalForceChanged externalForceChanged)
        {
            ExternalForce = externalForceChanged.ExternalForce;
        }
    }
}