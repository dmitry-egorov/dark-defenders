using DarkDefenders.Domain.Model.Entities.RigidBodies;
using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Entities.Weapons.Events
{
    internal class WeaponCreated : Created<Weapon>
    {
        private readonly RigidBody _rigidBody;

        public WeaponCreated(Weapon weapon, IStorage<Weapon> storage, RigidBody rigidBody) : base(weapon, storage)
        {
            _rigidBody = rigidBody;
        }

        protected override void ApplyTo(Weapon entity)
        {
            entity.Created(_rigidBody);
        }

        public override void Accept(IEventsReciever reciever)
        {
        }
    }
}