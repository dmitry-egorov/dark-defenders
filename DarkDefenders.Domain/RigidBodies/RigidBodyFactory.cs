using System.Collections.Generic;
using DarkDefenders.Domain.Clocks;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.RigidBodies.Events;
using DarkDefenders.Domain.Terrains;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;
using Infrastructure.Util;

namespace DarkDefenders.Domain.RigidBodies
{
    public class RigidBodyFactory : RootFactory<RigidBodyId, RigidBody, RigidBodyCreated>
    {
        private readonly IRepository<ClockId, Clock> _clockRepository;
        private readonly IRepository<TerrainId, Terrain> _terrainRepository;

        public RigidBodyFactory(IRepository<RigidBodyId, RigidBody> repository, IRepository<ClockId, Clock> clockRepository, IRepository<TerrainId, Terrain> terrainRepository) : base(repository)
        {
            _clockRepository = clockRepository;
            _terrainRepository = terrainRepository;
        }

        public IEnumerable<IDomainEvent> CreateRigidBody(RigidBodyId id, ClockId clockId, TerrainId terrainId, Momentum initialMomentum, Vector position, RigidBodyProperties properties)
        {
            AssertDoesntExist(id);

            return new RigidBodyCreated(id, clockId, terrainId, position, initialMomentum, properties).EnumerateOnce();
        }

        protected override RigidBody Handle(RigidBodyCreated created)
        {
            var clock = _clockRepository.GetById(created.ClockId);
            var world = _terrainRepository.GetById(created.TerrainId);
            var rigidBodyId = created.RootId;
            var initialMomentum = created.Momentum;
            var mass = created.Properties.Mass;
            var topHorizontalMomentum = created.Properties.TopHorizontalMomentum;
            var radius = created.Properties.BoundingBoxRadius;
            var boundingBox = new Box(created.Position, radius, radius);

            return new RigidBody(rigidBodyId, clock, world, initialMomentum, mass, topHorizontalMomentum, boundingBox);
        }
    }
}