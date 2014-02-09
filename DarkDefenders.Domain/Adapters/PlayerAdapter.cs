using DarkDefenders.Domain.Data.Other;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Adapters
{
    internal class PlayerAdapter : IPlayer
    {
        private readonly EntityAdapter<Creature> _creature;

        public PlayerAdapter(EntityAdapter<Creature> creatureAdapter)
        {
            _creature = creatureAdapter;
        }

        public void ChangeMovement(Movement movement)
        {
            _creature.Commit(x => x.ChangeMovement(movement));
        }

        public void Jump()
        {
            _creature.Commit(x => x.Jump());
        }

        public void Fire()
        {
            _creature.Commit(x => x.Fire());
        }
         
    }
}