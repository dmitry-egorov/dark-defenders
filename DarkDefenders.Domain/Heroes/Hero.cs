using System.Collections.Generic;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Heroes.Events;
using DarkDefenders.Domain.Heroes.States;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Heroes
{
    public class Hero: RootBase<HeroId, IHeroEventsReciever, IHeroEvent>, IHeroEventsReciever
    {
        private readonly Creature _creature;

        public IEnumerable<IDomainEvent> Think()
        {
            var events = _state.Update();

            foreach (var e in events) { yield return e; }
        }

        public void Recieve(StateChanged stateChanged)
        {
            _state = stateChanged.State;
        }

        internal Hero(HeroId id, Creature creature, HeroStateFactory stateFactory) : base(id)
        {
            _creature = creature;
            _state = stateFactory.CreateInitial();
        }

        private IHeroState _state;

        public IEnumerable<IDomainEvent> Kill()
        {
            yield return new HeroDestroyed(Id);

            var events = _creature.Kill();

            foreach (var e in events) { yield return e; }
        }
    }
}