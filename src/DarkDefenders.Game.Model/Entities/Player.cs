using DarkDefenders.Game.Model.Events;
using DarkDefenders.Game.Model.Other;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;
using JetBrains.Annotations;

namespace DarkDefenders.Game.Model.Entities
{
    [UsedImplicitly]
    public class Player : Entity<Player, IPlayerEvents>, IPlayerEvents
    {
        const string PropertiesId = "Player";

        private readonly Creature _creature;
        private readonly Weapon _weapon;
        private readonly RigidBody _rigidBody;

        public Player(Creature creature, Weapon weapon, RigidBody rigidBody)
        {
            _creature = creature;
            _weapon = weapon;
            _rigidBody = rigidBody;
        }

        public void Create(Vector initialPosition)
        {
            _rigidBody.Create(initialPosition, Momentum.Zero, PropertiesId);
            _creature.Create(_rigidBody, PropertiesId);
            _weapon.Create(_rigidBody);

            CreationEvent(x => x.Created(_creature));
        }

        public void ChangeMovement(Movement movement)
        {
            _creature.ChangeMovementTo(movement);
        }

        public void TryJump()
        {
            _creature.TryJump();
        }

        public void TryFire()
        {
            var direction = _creature.GetDirection();

            _weapon.TryFire(direction);
        }

        void IPlayerEvents.Created(Creature creature)
        {
        }
    }
}