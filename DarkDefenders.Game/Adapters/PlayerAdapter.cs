using DarkDefenders.Domain.Model.Entities.Players;
using DarkDefenders.Domain.Model.Other;
using DarkDefenders.Game.Interfaces;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Game.Adapters
{
    internal class PlayerAdapter : IPlayer
    {
        private readonly EntityAdapter<Player> _palyer;

        public PlayerAdapter(EntityAdapter<Player> palyer)
        {
            _palyer = palyer;
        }

        public void ChangeMovement(Movement movement)
        {
            _palyer.Commit(x => x.ChangeMovement(movement));
        }

        public void Jump()
        {
            _palyer.Commit(x => x.Jump());
        }

        public void Fire()
        {
            _palyer.Commit(x => x.Fire());
        }
    }
}