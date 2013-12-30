using DarkDefenders.Domain.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Projectiles.Events
{
    public interface IProjectileEvent: IRootEvent<IProjectileEventsReciever>, IDomainEvent
    {
    }
}