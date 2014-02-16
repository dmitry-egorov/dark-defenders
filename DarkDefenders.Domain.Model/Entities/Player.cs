using DarkDefenders.Domain.Model.Events;
using DarkDefenders.Domain.Model.Other;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities
{
    [UsedImplicitly]
    public class Player : Entity<Player, IPlayerEvents>, IPlayerEvents
    {
        private readonly Creature _creature;

        public Player(Creature creature)
        {
            _creature = creature;
        }

        public void Create(Vector initialPosition)
        {
            _creature.Create(initialPosition, "Player");

            CreationEvent(x => x.Created(_creature));
        }

        public void ChangeMovement(Movement movement)
        {
            _creature.ChangeMovementTo(movement);
        }

        public void Jump()
        {
            _creature.Jump();
        }

        public void Fire()
        {
            _creature.Fire();
        }

        void IPlayerEvents.Created(Creature creature)
        {
        }
    }
}