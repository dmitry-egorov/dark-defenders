using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Worlds.Events
{
    public class WorldDestroyed: Destroyed<WorldId, World, WorldDestroyed>, IDomainEvent
    {
        public WorldDestroyed(WorldId rootId) : base(rootId)
        {
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}