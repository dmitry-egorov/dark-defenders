using System.Collections.Generic;
using DarkDefenders.Domain.Model.Entities.Creatures;
using DarkDefenders.Domain.Model.Entities.Players.Events;
using DarkDefenders.Domain.Model.Entities.RigidBodies;
using DarkDefenders.Domain.Model.Other;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities.Players
{
    [UsedImplicitly]
    public class Player : Entity<Player>
    {
        private static readonly RigidBodyProperties _playersRigidBodyProperties = new RigidBodyProperties(0.4f, 1.0f, 40.0f);
        private static readonly CreatureProperties _playersAvatarProperties = new CreatureProperties(180.0f, 60.0f, _playersRigidBodyProperties);

        private readonly IStorage<Player> _storage;
        private readonly Creature _creature;
        

        public Player(IStorage<Player> storage, Creature creature)
        {
            _storage = storage;
            _creature = creature;
        }

        public IEnumerable<IEvent> Create(Vector initialPosition)
        {
            var events = _creature.Create(initialPosition, _playersAvatarProperties);

            foreach (var e in events) { yield return e; }

            yield return new PlayerCreated(this, _storage, _creature);
        }

        public IEnumerable<IEvent> ChangeMovement(Movement movement)
        {
            return _creature.ChangeMovementTo(movement);
        }

        public IEnumerable<IEvent> Jump()
        {
            return _creature.Jump();
        }

        public IEnumerable<IEvent> Fire()
        {
            return _creature.Fire();
        }

        internal void Created(Creature creature)
        {
        }
    }
}