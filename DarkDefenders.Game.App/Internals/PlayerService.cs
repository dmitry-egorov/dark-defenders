using DarkDefenders.Game.App.Interfaces;
using DarkDefenders.Game.Model.Entities;
using DarkDefenders.Game.Model.Other;

namespace DarkDefenders.Game.App.Internals
{
    internal class PlayerService : IPlayerService
    {
        private readonly Player _palyer;

        public PlayerService(Player palyer)
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