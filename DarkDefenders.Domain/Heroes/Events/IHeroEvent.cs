using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Heroes.Events
{
    public interface IHeroEvent: IRootEvent<IHeroEventsReciever>
    {
    }
}