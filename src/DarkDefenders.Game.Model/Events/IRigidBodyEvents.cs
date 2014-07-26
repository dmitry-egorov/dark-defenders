using DarkDefenders.Game.Model.Entities;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Infrastructure.Physics;

namespace DarkDefenders.Game.Model.Events
{
    public interface IRigidBodyEvents : IEntityEvents
    {
        void Created(RigidBody rigidBody, Vector initialPosition, Momentum initialMomentum, string properties);
        void Accelerated(Momentum newMomentum);
        void Moved(Vector newPosition);
        void ExternalForceChanged(Force externalForce);
    }
}