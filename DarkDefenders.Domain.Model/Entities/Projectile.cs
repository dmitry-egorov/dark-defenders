using System.Collections.Generic;
using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;
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

        public Projectile(IProjectileEvents external, IStorage<Projectile> storage, RigidBody rigidBody)
            : base(external, storage)
        {
            _rigidBody = rigidBody;
        }

        public IEnumerable<IEvent> Create(Vector position, Momentum momentum)
        {
            var events = _rigidBody.Create(position, momentum, "Projectile");

            foreach (var e in events) { yield return e; }

            yield return CreationEvent(x => x.Created(_rigidBody.Id));
        }

        public IEnumerable<IEvent> CheckForHit()
        {
            if (!_rigidBody.IsTouchingAnyWalls())
            {
                yield break;
            }

            yield return DestructionEvent();

            var events = _rigidBody.Destroy();

            foreach (var e in events) { yield return e; }
        }

        void IProjectileEvents.Created(IdentityOf<RigidBody> rigidBody)
        {
        }

        void IEntityEvents.Destroyed()
        {
        }
    }
}