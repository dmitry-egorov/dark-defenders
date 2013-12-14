using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Players.Events
{
    public class PlayerMoved : EventBase<PlayerId, PlayerMoved>, IPlayerEvent
    {
        public Vector NewPosition { get; private set; }

        public PlayerMoved(PlayerId id, Vector newPosition): base(id)
        {
            NewPosition = newPosition;
        }

        protected override string EventToString()
        {
            return "Player moved: {0} {1}".FormatWith(RootId, NewPosition);
        }

        protected override bool EventEquals(PlayerMoved other)
        {
            return NewPosition == other.NewPosition;
        }

        protected override int GetEventHashCode()
        {
            return NewPosition.GetHashCode();
        }

        public void ApplyTo(IPlayerEventsReciever reciever)
        {
            reciever.Apply(this);
        }
    }
}