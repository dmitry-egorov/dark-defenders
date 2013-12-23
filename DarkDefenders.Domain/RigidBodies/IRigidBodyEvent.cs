using DarkDefenders.Domain.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.RigidBodies
{
    public interface IRigidBodyEvent : IRootEvent<RigidBodyId, IRigidBodyEventsReciever>, IDomainEvent
    {
    }
}