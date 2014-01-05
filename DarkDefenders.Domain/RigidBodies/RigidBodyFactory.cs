using System.Collections.Generic;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.RigidBodies.Events;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Math.Physics;
using Infrastructure.Util;

namespace DarkDefenders.Domain.RigidBodies
{
    public class RigidBodyFactory : RootFactory<RigidBodyId, RigidBody, RigidBodyCreated>
    {
        private readonly IRepository<WorldId, World> _worldRepository;

        public RigidBodyFactory(IRepository<RigidBodyId, RigidBody> repository, IRepository<WorldId, World> worldRepository) : base(repository)
        {
            _worldRepository = worldRepository;
        }

        public IEnumerable<IDomainEvent> CreateRigidBody(RigidBodyId id, WorldId worldId, Momentum initialMomentum, Vector position, RigidBodyProperties properties)
        {
            AssertDoesntExist(id);

            return new RigidBodyCreated(id, worldId, position, initialMomentum, properties).EnumerateOnce();
        }

        protected override RigidBody Handle(RigidBodyCreated created)
        {
            var world = _worldRepository.GetById(created.WorldId);
            var rigidBodyId = created.RootId;
            var initialMomentum = created.Momentum;
            var mass = created.Properties.Mass;
            var topHorizontalMomentum = created.Properties.TopHorizontalMomentum;
            var radius = created.Properties.BoundingBoxRadius;
            var boundingBox = new Box(created.Position, radius, radius);

            return new RigidBody(rigidBodyId, world, initialMomentum, mass, topHorizontalMomentum, boundingBox);
        }
    }
}