using System.Collections.Generic;
using DarkDefenders.Domain.RigidBodies.Events;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.RigidBodies
{
    public class RigidBodyFactory: Factory<RigidBodyId, RigidBody>, IFactory<RigidBody, RigidBodyCreated>
    {
        private readonly IRepository<WorldId, World> _worldRepository;

        public RigidBodyFactory(IRepository<RigidBodyId, RigidBody> repository, IRepository<WorldId, World> worldRepository) : base(repository)
        {
            _worldRepository = worldRepository;
        }

        public IEnumerable<IEvent> CreateRigidBody(RigidBodyId id, WorldId worldId, Vector position, double radius, Vector initialMomentum, double mass)
        {
            AssertDoesntExist(id);
            
            var boundingCircle = new Circle(position, radius);
            return new RigidBodyCreated(id, worldId, boundingCircle, initialMomentum, mass).EnumerateOnce();
        }

        RigidBody IFactory<RigidBody, RigidBodyCreated>.Handle(RigidBodyCreated created)
        {
            var world = _worldRepository.GetById(created.WorldId);
            return new RigidBody(created.RootId, world, created.InitialMomentum, created.Mass, created.BoundingCircle);
        }
    }
}