using DarkDefenders.Domain.Players;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.RigidBodies
{
    public interface IRigidBodyEvent : IRootEvent<RigidBodyId, IRigidBodyEventsReciever>
    {
    }
}