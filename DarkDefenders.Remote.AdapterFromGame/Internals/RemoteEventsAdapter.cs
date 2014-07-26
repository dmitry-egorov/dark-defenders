using System.Collections.Generic;
using DarkDefenders.Game.Model.Entities;
using DarkDefenders.Kernel.Model;
using DarkDefenders.Remote.Model;
using Infrastructure.Math;

namespace DarkDefenders.Remote.AdapterFromGame.Internals
{
    internal class RemoteEventsAdapter
    {
        private readonly RemoteState _reciever;
        
        private readonly Dictionary<RigidBody, Vector> _initialPositionsMap = new Dictionary<RigidBody, Vector>();
        private readonly Dictionary<Creature, RigidBody> _rigidBodiesMap = new Dictionary<Creature, RigidBody>();

        public RemoteEventsAdapter(RemoteState reciever)
        {
            _reciever = reciever;
        }

        public void RigidBodyCreated(RigidBody rigidBody, Vector position)
        {
            _initialPositionsMap[rigidBody] = position;
        }

        public void RigidBodyDestroyed(RigidBody rigidBody)
        {
            _reciever.Destroyed(rigidBody.Id.ToRemote());
        }

        public void Moved(RigidBody rigidBody, Vector newPosition)
        {
            _reciever.Moved(rigidBody.Id.ToRemote(), newPosition);
        }

        public void ChangedDirection(RigidBody rigidBody, Direction newDirection)
        {
            _reciever.ChangedDirection(rigidBody.Id.ToRemote(), newDirection);
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
            var initialPosition = _initialPositionsMap[rigidBody];

            _initialPositionsMap.Remove(rigidBody);
            _reciever.Created(rigidBody.Id.ToRemote(), initialPosition, Direction.Right, type);
        }

        private void RigidBodyCreatedInternal(Creature creature, RemoteEntityType type)
        {
            var rigidBody = _rigidBodiesMap[creature];
            var initialPosition = _initialPositionsMap[rigidBody];

            _initialPositionsMap.Remove(rigidBody);
            _reciever.Created(rigidBody.Id.ToRemote(), initialPosition, Direction.Right, type);
        }
    }
}