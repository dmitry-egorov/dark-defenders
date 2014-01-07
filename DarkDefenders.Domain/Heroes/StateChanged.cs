using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Heroes.Events;
using DarkDefenders.Domain.Heroes.States;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Heroes
{
    public class StateChanged : EventBase<HeroId, StateChanged>, IHeroEvent
    {
        public IHeroState State { get; private set; }

        public StateChanged(HeroId rootId, IHeroState state) : base(rootId)
        {
            State = state;
        }

        public void ApplyTo(IHeroEventsReciever reciever)
        {
            reciever.Recieve(this);
        }

        public void ApplyTo(IDomainEventsReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}