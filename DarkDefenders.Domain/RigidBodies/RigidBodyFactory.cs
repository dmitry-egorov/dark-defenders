using System.Collections.Generic;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.Domain.RigidBodies
{
    public class RigidBodyFactory
    {
        private readonly IRepository<RigidBodyId, RigidBody> _rigidBodyRepository;

        public RigidBodyFactory(IRepository<RigidBodyId, RigidBody> rigidBodyRepository)
        {
            _rigidBodyRepository = rigidBodyRepository;
        }

        public IEnumerable<IRigidBodyEvent> Create(WorldId worldId, RigidBodyId rigidBodyId, Circle boundingCircle)
        {
            var rigidBody = _rigidBodyRepository.GetById(rigidBodyId);

            return rigidBody.Create(worldId, boundingCircle);
        }
    }
}