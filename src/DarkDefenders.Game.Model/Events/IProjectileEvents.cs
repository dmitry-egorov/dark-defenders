using DarkDefenders.Game.Model.Entities;
using Infrastructure.DDDES;

namespace DarkDefenders.Game.Model.Events
{
    public interface IProjectileEvents : IEntityEvents
    {
        void Created(RigidBody rigidBody);
    }
}