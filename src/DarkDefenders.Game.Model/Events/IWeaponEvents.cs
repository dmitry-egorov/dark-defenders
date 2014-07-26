using DarkDefenders.Game.Model.Entities;
using Infrastructure.DDDES;

namespace DarkDefenders.Game.Model.Events
{
    public interface IWeaponEvents : IEntityEvents
    {
        void Created(RigidBody rigidBody);
    }
}