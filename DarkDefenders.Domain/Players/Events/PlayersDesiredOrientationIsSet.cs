using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Players.Events
{
    public class PlayersDesiredOrientationIsSet : EventBase<PlayerId, PlayersDesiredOrientationIsSet>, IPlayerEvent
    {
        public Vector Orientation { get; private set; }

        public PlayersDesiredOrientationIsSet(PlayerId playerId, Vector orientation) : base(playerId)
        {
            Orientation = orientation;
        }

        protected override string EventToString()
        {
            return "Players desired orientation changed: {0}, {1}".FormatWith(RootId, Orientation);
        }

        protected override bool EventEquals(PlayersDesiredOrientationIsSet other)
        {
            return Orientation == other.Orientation;
        }

        protected override int GetEventHashCode()
        {
            return Orientation.GetHashCode();
        }

        public void ApplyTo(IPlayerEventsReciever reciever)
        {
            reciever.Apply(this);
        }
    }
}