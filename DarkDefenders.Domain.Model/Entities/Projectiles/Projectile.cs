using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Model.Entities.Projectiles.Events;
using DarkDefenders.Domain.Model.Entities.RigidBodies;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities.Projectiles
{
    [UsedImplicitly]
    public class Projectile : Entity<Projectile>
    {
        public const double Mass = 0.001;
        private const double BoundingBoxRadius = 0.1;

        private readonly IStorage<Projectile> _storage;

        private readonly RigidBody _rigidBody;

        public Projectile(IStorage<Projectile> storage, RigidBody rigidBody)
        {
            _storage = storage;
            _rigidBody = rigidBody;
        }

        public IEnumerable<IEvent> Create(Vector position, Momentum momentum)
        {
            var radius = BoundingBoxRadius;
            var mass = Mass;
            var topHorizontalMomentum = Math.Abs(momentum.Value.X);
            var properties = new RigidBodyProperties((float)radius, (float)mass, (float)topHorizontalMomentum);

            var events = _rigidBody.Create(position, momentum, properties);

            foreach (var e in events) { yield return e; }

            yield return new ProjectileCreated(_storage, this, _rigidBody);
        }

        public IEnumerable<IEvent> CheckForHit()
        {
            if (!IsHit())
            {
                yield break;
            }

            yield return new ProjectileDestroyed(this, _storage);

            var events = _rigidBody.Destroy();

            foreach (var e in events) { yield return e; }
        }

        internal void Created(RigidBody rigidBody)
        {
        }

        private bool IsHit()
        {
            return _rigidBody.IsTouchingAnyWalls();
        }
    }
}