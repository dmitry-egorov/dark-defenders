using System;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Worlds.Events
{
    public class HeroesSpawned : EventBase<WorldId, HeroesSpawned>, IWorldEvent
    {
        public TimeSpan Time { get; private set; }
        public CreatureId CreatureId { get; private set; }

        public HeroesSpawned(WorldId worldId, TimeSpan time, CreatureId creatureId)
            : base(worldId)
        {
            CreatureId = creatureId;
            Time = time;
        }

        protected override string ToStringInternal()
        {
            return "Heroes spawned: {0}, {1}, {2}".FormatWith(RootId, Time, CreatureId);
        }

        protected override bool EventEquals(HeroesSpawned other)
        {
            return Time.Equals(other.Time)
                && CreatureId.Equals(other.CreatureId);
        }

        protected override int GetEventHashCode()
        {
            var hashCode = Time.GetHashCode();
            hashCode = (hashCode * 397) ^ CreatureId.GetHashCode();
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