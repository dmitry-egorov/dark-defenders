using System;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Heroes;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Worlds.Events
{
    public class HeroesSpawned : EventBase<WorldId, HeroesSpawned>, IWorldEvent
    {
        public TimeSpan Time { get; private set; }
        public HeroId HeroId { get; private set; }

        public HeroesSpawned(WorldId worldId, TimeSpan time, HeroId heroId)
            : base(worldId)
        {
            HeroId = heroId;
            Time = time;
        }

        protected override string ToStringInternal()
        {
            return "Heroes spawned: {0}, {1}, {2}".FormatWith(RootId, Time, HeroId);
        }

        protected override bool EventEquals(HeroesSpawned other)
        {
            return Time.Equals(other.Time)
                && HeroId.Equals(other.HeroId);
        }

        protected override int GetEventHashCode()
        {
            var hashCode = Time.GetHashCode();
            hashCode = (hashCode * 397) ^ HeroId.GetHashCode();
            return hashCode;
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }

        public void ApplyTo(IWorldEventsReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}