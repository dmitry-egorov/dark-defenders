using DarkDefenders.Game.Model.Other;

namespace DarkDefenders.Game.App.Interfaces
{
    public interface IPlayerService
    {
        void ChangeMovement(Movement movement);
        void Jump();
        void Fire();
    }
}