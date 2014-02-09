using DarkDefenders.Domain.Data.Other;

namespace DarkDefenders.Domain.Interfaces
{
    public interface IPlayer
    {
        void ChangeMovement(Movement movement);
        void Jump();
        void Fire();
    }
}