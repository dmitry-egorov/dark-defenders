using DarkDefenders.Domain.Model.Entities;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Events
{
    public interface IWeaponEvents : IEntityEvents
    {
        void Created(RigidBody rigidBody);
    }
}