using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Worlds.Events
{
    public class WorldRemoved: Removed<WorldId, WorldRemoved>, IDomainEvent
    {
        public WorldRemoved(WorldId rootId) : base(rootId)
        {
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}