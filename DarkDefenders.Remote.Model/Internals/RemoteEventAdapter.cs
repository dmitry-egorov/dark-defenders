using System.Collections.Generic;
using DarkDefenders.Domain.Model.Entities;
using DarkDefenders.Remote.Model.Interface;
using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.Remote.Model.Internals
{
    internal class RemoteEventAdapter
    {
        private readonly IRemoteEvents _reciever;
        
        private readonly Dictionary<IdentityOf<RigidBody>, Vector> _positionsMap = new Dictionary<IdentityOf<RigidBody>, Vector>();
        private readonly Dictionary<IdentityOf<Creature>, IdentityOf<RigidBody>> _rigidBodiesMap = new Dictionary<IdentityOf<Creature>, IdentityOf<RigidBody>>();

        public RemoteEventAdapter(IRemoteEvents reciever)
        {
            _reciever = reciever;
        }

        public void RigidBodyCreated(IdentityOf<RigidBody> rigidBodyId, Vector position)
        {
            _positionsMap[rigidBodyId] = position;
        }

        public void RigidBodyDestroyed(IdentityOf<RigidBody> rigidBodyId)
        {
            _positionsMap.Remove(rigidBodyId);
            _reciever.Destroyed(rigidBodyId);
        }

        public void Moved(IdentityOf<RigidBody> rigidBodyId, Vector newPosition)
        {
            _reciever.Moved(rigidBodyId, newPosition);
        }

        public void CreatureCreated(IdentityOf<Creature> creatureId, IdentityOf<RigidBody> rigidBodyId)
        {
            _rigidBodiesMap[creatureId] = rigidBodyId;
        }

        public void HeroCreated(IdentityOf<Creature> creatureId)
        {
            RigidBodyCreatedInternal(creatureId, RemoteEntityType.Hero);
        }

        public void PlayerCreated(IdentityOf<Creature> creatureId)
        {
            RigidBodyCreatedInternal(creatureId, RemoteEntityType.Player);
        }

        public void ProjectileCreated(IdentityOf<RigidBody> rigidBodyId)
        {
            RigidBodyCreatedInternal(rigidBodyId, RemoteEntityType.Projectile);
        }

        private void RigidBodyCreatedInternal(IdentityOf<RigidBody> rigidBodyId, RemoteEntityType type)
        {
            var initialPosition = _positionsMap[rigidBodyId];

            _reciever.Created(rigidBodyId, initialPosition, type);
        }

        private void RigidBodyCreatedInternal(IdentityOf<Creature> creatureId, RemoteEntityType type)
        {
            var rigidBodyId = _rigidBodiesMap[creatureId];
            var initialPosition = _positionsMap[rigidBodyId];

            _reciever.Created(rigidBodyId, initialPosition, type);
        }
    }
}