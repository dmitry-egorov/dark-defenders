namespace DarkDefenders.Domain.Players.Events
{
    public interface IPlayerEventsReciever
    {
        void Recieve(MovementForceDirectionChanged movementForceDirectionChanged);
        void Recieve(PlayerFired playerFired);
    }
}