namespace DarkDefenders.Domain.Creatures.Events
{
    public interface ICreatureEventsReciever
    {
        void Recieve(MovementChanged movementChanged);
        void Recieve(CreatureFired creatureFired);
    }
}