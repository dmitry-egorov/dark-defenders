using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Players.Events
{
    public class PlayerFired: EventBase<PlayerId, PlayerFired>, IPlayerEvent
    {
        public double Time { get; private set; }

        public PlayerFired(PlayerId rootId, double time) : base(rootId)
        {
            Time = time;
        }

        protected override string ToStringInternal()
        {
            return "Player fired: {0}, {1}".FormatWith(RootId, Time);
        }

        protected override bool EventEquals(PlayerFired other)
        {
            return Time.Equals(other.Time);
        }

        protected override int GetEventHashCode()
        {
            return Time.GetHashCode();
        }

        public void ApplyTo(IPlayerEventsReciever reciever)
        {
            reciever.Recieve(this);
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}