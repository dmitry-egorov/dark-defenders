using System;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Heroes;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Worlds.Events
{
    public class HeroSpawned : EventBase<WorldId, HeroSpawned>, IWorldEvent
    {
        public TimeSpan Time { get; private set; }
        public HeroId HeroId { get; private set; }

        public HeroSpawned(WorldId worldId, TimeSpan time, HeroId heroId)
            : base(worldId)
        {
            HeroId = heroId;
            Time = time;
        }

        public void ApplyTo(IDomainEventsReciever reciever)
        {
            reciever.Recieve(this);
        }

        public void ApplyTo(IWorldEventsReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}