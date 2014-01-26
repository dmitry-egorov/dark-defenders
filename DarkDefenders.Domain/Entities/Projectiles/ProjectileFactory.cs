using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Entities.Projectiles.Events;
using DarkDefenders.Domain.Entities.RigidBodies;
using DarkDefenders.Dtos.Entities.RigidBodies;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Entities.Projectiles
{
    internal class ProjectileFactory
    {
        private readonly RigidBodyFactory _rigidBodyFactory;
        private readonly IStorage<Projectile> _storage;

        public ProjectileFactory(IStorage<Projectile> storage, RigidBodyFactory rigidBodyFactory)
        {
            _rigidBodyFactory = rigidBodyFactory;
            _storage = storage;
        }

        public IEnumerable<IEvent> Create(Vector position, Momentum momentum)
        {
            var container = new Container<RigidBody>();

            var events = CreateProjectileRigidBody(container, position, momentum);

            return events.ConcatItem(new ProjectileCreated(_storage, container));
        }

        private IEnumerable<IEvent> CreateProjectileRigidBody(IStorage<RigidBody> storage, Vector position, Momentum momentum)
        {
            var radius = Projectile.BoundingBoxRadius;
            var mass = Projectile.Mass;
            var topHorizontalMomentum = Math.Abs(momentum.Value.X);
            var properties = new RigidBodyProperties(radius, mass, topHorizontalMomentum);
            var rigidBodyInitialProperties = new RigidBodyInitialProperties(momentum, position, properties);

            return _rigidBodyFactory.Create(storage, rigidBodyInitialProperties);
        }
    }
}