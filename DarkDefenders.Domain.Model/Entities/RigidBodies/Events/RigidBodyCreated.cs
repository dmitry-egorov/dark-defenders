using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Infrastructure.Physics;

namespace DarkDefenders.Domain.Model.Entities.RigidBodies.Events
{
    internal class RigidBodyCreated : Created<RigidBody>
    {
        private readonly RigidBody _rigidBody;
        private readonly Momentum _initialMomentum;
        private readonly Vector _initialPosition;
        private readonly RigidBodyProperties _properties;

        public RigidBodyCreated(RigidBody rigidBody, IStorage<RigidBody> storage, Vector initialPosition, Momentum initialMomentum, RigidBodyProperties properties)
            : base(rigidBody, storage)
        {
            _rigidBody = rigidBody;
            _initialPosition = initialPosition;
            _initialMomentum = initialMomentum;
            _properties = properties;
        }

        protected override void ApplyTo(RigidBody entity)
        {
            entity.Created(_initialPosition, _initialMomentum, _properties);
        }

        public override void Accept(IEventsReciever reciever)
        {
            reciever.RigidBodyCreated(_rigidBody.Id, _initialPosition);
        }
    }
}