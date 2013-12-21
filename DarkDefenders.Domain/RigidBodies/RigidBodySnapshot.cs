using DarkDefenders.Domain.RigidBodies.Events;
using DarkDefenders.Domain.Worlds;
using Infrastructure.Math;

namespace DarkDefenders.Domain.RigidBodies
{
    public class RigidBodySnapshot : IRigidBodyEventsReciever
    {
        public WorldId WorldId { get; private set; }
        public Circle BoundingCircle { get; private set; }
        public Vector Momentum { get; private set; }
        public Vector ExternalForce { get; private set; }
        
        public void Apply(RigidBodyCreated rigidBodyCreated)
        {
            WorldId = rigidBodyCreated.WorldId;
            BoundingCircle = rigidBodyCreated.BoundingCircle;
            Momentum = Vector.Zero;
            ExternalForce = Vector.Zero;
        }

        public void Apply(Moved moved)
        {
            BoundingCircle = BoundingCircle.ChangePosition(moved.NewPosition);
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