using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Projectiles.Events;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Math.Physics;

namespace DarkDefenders.Domain.Projectiles
{
    public class ProjectileFactory : RootFactory<ProjectileId, Projectile, ProjectileCreated>
    {
        private readonly IRepository<RigidBodyId, RigidBody> _rigidBodyRepositroy;
        private readonly RigidBodyFactory _rigidBodyFactory;

        public ProjectileFactory(IRepository<ProjectileId, Projectile> repository, IRepository<RigidBodyId, RigidBody> rigidBodyRepositroy, RigidBodyFactory rigidBodyFactory) : base(repository)
        {
            _rigidBodyRepositroy = rigidBodyRepositroy;
            _rigidBodyFactory = rigidBodyFactory;
        }

        public IEnumerable<IDomainEvent> Create(ProjectileId projectileId, WorldId worldId, Vector position, Momentum momentum)
        {
            var rigidBodyId = new RigidBodyId();

            var events = CreateProjectileRigidBody(rigidBodyId, worldId, position, momentum);

            foreach (var e in events) { yield return e; }

            yield return new ProjectileCreated(projectileId, rigidBodyId);
        }

        private IEnumerable<IDomainEvent> CreateProjectileRigidBody(RigidBodyId rigidBodyId, WorldId worldId, Vector position, Momentum momentum)
        {
            var radius = Projectile.BoundingBoxRadius;
            var mass = Projectile.Mass;
            var topHorizontalMomentum = Math.Abs(momentum.Value.X);
            var properties = new RigidBodyProperties(radius, mass, topHorizontalMomentum);

            return _rigidBodyFactory.CreateRigidBody(rigidBodyId, worldId, momentum, position, properties);
        }

        protected override Projectile Handle(ProjectileCreated creationEvent)
        {
            var rigidBody = _rigidBodyRepositroy.GetById(creationEvent.RigidBodyId);

            return new Projectile(creationEvent.RootId, rigidBody);
        }
    }
}