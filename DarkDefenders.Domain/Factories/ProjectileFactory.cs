using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Data.Entities.RigidBodies;
using DarkDefenders.Domain.Entities.Projectiles;
using DarkDefenders.Domain.Entities.Projectiles.Events;
using DarkDefenders.Domain.Entities.RigidBodies;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;

namespace DarkDefenders.Domain.Factories
{
    internal class ProjectileFactory: Factory<Projectile>
    {
        private readonly RigidBodyFactory _rigidBodyFactory;

        public ProjectileFactory(IStorage<Projectile> storage, RigidBodyFactory rigidBodyFactory) : base(storage)
        {
            _rigidBodyFactory = rigidBodyFactory;
        }

        public ICreation<Projectile> Create(Vector position, Momentum momentum)
        {
            return GetCreation(s => YieldEvents(s, position, momentum));
        }

        private IEnumerable<IEvent> YieldEvents(IStorage<Projectile> storage, Vector position, Momentum momentum)
        {
            var creation = CreateProjectileRigidBody(position, momentum);

            foreach (var e in creation) { yield return e; }

            yield return new ProjectileCreated(storage, creation);
        }

        private ICreation<RigidBody> CreateProjectileRigidBody(Vector position, Momentum momentum)
        {
            var radius = Projectile.BoundingBoxRadius;
            var mass = Projectile.Mass;
            var topHorizontalMomentum = Math.Abs(momentum.Value.X);
            var properties = new RigidBodyProperties((float) radius, (float) mass, (float) topHorizontalMomentum);
            var rigidBodyInitialProperties = new RigidBodyInitialProperties(momentum.ToData(), position.ToData(), properties);

            return _rigidBodyFactory.Create(rigidBodyInitialProperties);
        }
    }
}