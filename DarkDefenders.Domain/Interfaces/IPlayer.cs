using DarkDefenders.Domain.Other;

namespace DarkDefenders.Domain.Interfaces
{
    public interface IPlayer
    {
        void ChangeMovement(Movement movement);
        void Jump();
        void Fire();
    }
}