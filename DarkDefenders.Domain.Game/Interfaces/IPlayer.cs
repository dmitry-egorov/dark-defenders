using DarkDefenders.Domain.Model.Other;

namespace DarkDefenders.Domain.Game.Interfaces
{
    public interface IPlayer
    {
        void ChangeMovement(Movement movement);
        void Jump();
        void Fire();
    }
}