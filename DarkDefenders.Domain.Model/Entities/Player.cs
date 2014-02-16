using System.Collections.Generic;
using DarkDefenders.Domain.Model.Events;
using DarkDefenders.Domain.Model.Other;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities
{
    [UsedImplicitly]
    public class Player : Entity<Player, IPlayerEvents>, IPlayerEvents
    {
        private readonly Creature _creature;

        public Player(IPlayerEvents external, IStorage<Player> storage, Creature creature) 
            : base(external, storage)
        {
            _creature = creature;
        }

        public IEnumerable<IEvent> Create(Vector initialPosition)
        {
            var events = _creature.Create(initialPosition, "Player");

            foreach (var e in events) { yield return e; }

            yield return CreationEvent(x => x.Created(_creature.Id));
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

        void IPlayerEvents.Created(IdentityOf<Creature> creatureId)
        {
        }

        void IEntityEvents.Destroyed()
        {
        }
    }
}