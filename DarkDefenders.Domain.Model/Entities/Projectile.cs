using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities
{
    [UsedImplicitly]
    public class Projectile : Entity<Projectile, IProjectileEvents>, IProjectileEvents
    {
        public const double Mass = 0.001;

        private readonly RigidBody _rigidBody;

        public Projectile(RigidBody rigidBody)
        {
            _rigidBody = rigidBody;
        }

        public void Create(Vector position, Momentum momentum)
        {
            _rigidBody.Create(position, momentum, "Projectile");

            CreationEvent(x => x.Created(_rigidBody));
        }

        public void CheckForHit()
        {
            if (!_rigidBody.IsTouchingAnyWalls())
            {
                return;
            }

            DestructionEvent();

            _rigidBody.Destroy();
        }

        void IProjectileEvents.Created(RigidBody rigidBody)
        {
        }
    }
}