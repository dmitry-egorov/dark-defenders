using System.Collections.Generic;
using DarkDefenders.Domain.Model.Entities;
using DarkDefenders.Remote.Model.Interface;
using Infrastructure.Math;

namespace DarkDefenders.Remote.Model.Internals
{
    internal class RemoteEventAdapter
    {
        private readonly IRemoteEvents _reciever;
        
        private readonly Dictionary<RigidBody, Vector> _positionsMap = new Dictionary<RigidBody, Vector>();
        private readonly Dictionary<Creature, RigidBody> _rigidBodiesMap = new Dictionary<Creature, RigidBody>();

        public RemoteEventAdapter(IRemoteEvents reciever)
        {
            _reciever = reciever;
        }

        public void RigidBodyCreated(RigidBody rigidBody, Vector position)
        {
            _positionsMap[rigidBody] = position;
        }

        public void RigidBodyDestroyed(RigidBody rigidBody)
        {
            _positionsMap.Remove(rigidBody);

            _reciever.Destroyed(rigidBody.Id);
        }

        public void Moved(RigidBody rigidBody, Vector newPosition)
        {
            _reciever.Moved(rigidBody.Id, newPosition);
        }

        public void CreatureCreated(Creature creature, RigidBody rigidBody)
        {
            _rigidBodiesMap[creature] = rigidBody;
        }

        public void HeroCreated(Creature creature)
        {
            RigidBodyCreatedInternal(creature, RemoteEntityType.Hero);
        }

        public void PlayerCreated(Creature creature)
        {
            RigidBodyCreatedInternal(creature, RemoteEntityType.Player);
        }

        public void ProjectileCreated(RigidBody rigidBody)
        {
            RigidBodyCreatedInternal(rigidBody, RemoteEntityType.Projectile);
        }

        private void RigidBodyCreatedInternal(RigidBody rigidBody, RemoteEntityType type)
        {
            var initialPosition = _positionsMap[rigidBody];

            _reciever.Created(rigidBody.Id, initialPosition, type);
        }

        private void RigidBodyCreatedInternal(Creature creature, RemoteEntityType type)
        {
            var rigidBody = _rigidBodiesMap[creature];
            var initialPosition = _positionsMap[rigidBody];

            _reciever.Created(rigidBody.Id, initialPosition, type);
        }
    }
}