using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Interfaces;
using DarkDefenders.Dtos.Other;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Adapters
{
    internal class PlayerAdapter : IPlayer
    {
        private readonly RootAdapter<Creature> _creature;

        public PlayerAdapter(RootAdapter<Creature> creatureAdapter)
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