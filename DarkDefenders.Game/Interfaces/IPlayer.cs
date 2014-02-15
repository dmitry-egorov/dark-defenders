using DarkDefenders.Domain.Model.Other;

namespace DarkDefenders.Game.Interfaces
{
    public interface IPlayer
    {
        void ChangeMovement(Movement movement);
        void Jump();
        void Fire();
    }
}