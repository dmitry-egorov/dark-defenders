using DarkDefenders.Domain.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.RigidBodies.Events
{
    public interface IRigidBodyEvent : IRootEvent<IRigidBodyEventsReciever>, IDomainEvent
    {
    }
}