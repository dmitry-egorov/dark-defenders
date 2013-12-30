using System.Collections.Generic;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Players;
using DarkDefenders.Domain.Projectiles.Events;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

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

        public IEnumerable<IDomainEvent> Create(ProjectileId projectileId, PlayerId playerId, WorldId worldId, Vector position, Vector momentum)
        {
            var rigidBodyId = new RigidBodyId();

            var events = CreateProjectileRigidBody(rigidBodyId, position, worldId, momentum);

            foreach (var e in events) { yield return e; }

            yield return new ProjectileCreated(projectileId, rigidBodyId);
        }

        private IEnumerable<IDomainEvent> CreateProjectileRigidBody(RigidBodyId rigidBodyId, Vector position, WorldId worldId, Vector momentum)
        {
            return _rigidBodyFactory.CreateRigidBody(rigidBodyId, worldId, position, Projectile.BoundingCircleRadius, momentum, Projectile.Mass);
        }

        protected override Projectile Handle(ProjectileCreated creationEvent)
        {
            var rigidBody = _rigidBodyRepositroy.GetById(creationEvent.RigidBodyId);

            return new Projectile(creationEvent.RootId, rigidBody);
        }
    }
}