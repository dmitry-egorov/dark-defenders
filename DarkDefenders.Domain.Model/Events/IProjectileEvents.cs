using DarkDefenders.Domain.Model.Entities;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Events
{
    public interface IProjectileEvents : IEntityEvents
    {
        void Created(RigidBody rigidBody);
    }
}