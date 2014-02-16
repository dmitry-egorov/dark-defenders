using DarkDefenders.Domain.Game.Interfaces;
using DarkDefenders.Domain.Model.Entities;
using DarkDefenders.Domain.Model.Other;

namespace DarkDefenders.Domain.Game.Adapters
{
    internal class PlayerAdapter : IPlayer
    {
        private readonly Player _palyer;

        public PlayerAdapter(Player palyer)
        {
            _palyer = palyer;
        }

        public void ChangeMovement(Movement movement)
        {
            _palyer.ChangeMovement(movement);
        }

        public void Jump()
        {
            _palyer.Jump();
        }

        public void Fire()
        {
            _palyer.Fire();
        }
    }
}