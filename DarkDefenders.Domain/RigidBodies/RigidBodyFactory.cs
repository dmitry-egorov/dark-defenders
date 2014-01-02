using System.Collections.Generic;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.RigidBodies.Events;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
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

        public IEnumerable<IDomainEvent> CreateRigidBody(RigidBodyId id, WorldId worldId, Vector initialMomentum, double mass, double topHorizontalMomentum, Box boundingBox)
        {
            AssertDoesntExist(id);

            return new RigidBodyCreated(id, worldId, boundingBox, initialMomentum, mass, topHorizontalMomentum).EnumerateOnce();
        }

        protected override RigidBody Handle(RigidBodyCreated created)
        {
            var world = _worldRepository.GetById(created.WorldId);
            return new RigidBody(created.RootId, world, created.InitialMomentum, created.Mass, created.TopHorizontalMomentum, created.BoundingBox);
        }
    }
}