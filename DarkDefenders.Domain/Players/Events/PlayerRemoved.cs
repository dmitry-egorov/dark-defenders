using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Players.Events
{
    public class PlayerRemoved: Removed<PlayerId, PlayerRemoved>, IDomainEvent
    {
        public PlayerRemoved(PlayerId rootId) : base(rootId)
        {
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}