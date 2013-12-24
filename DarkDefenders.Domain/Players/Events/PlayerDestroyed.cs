using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Players.Events
{
    public class PlayerDestroyed: Destroyed<PlayerId, Player, PlayerDestroyed>, IDomainEvent
    {
        public PlayerDestroyed(PlayerId rootId) : base(rootId)
        {
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}