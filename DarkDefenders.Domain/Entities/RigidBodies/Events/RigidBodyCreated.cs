using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Entities.Terrains;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Infrastructure.Physics;

namespace DarkDefenders.Domain.Entities.RigidBodies.Events
{
    internal class RigidBodyCreated : Created<RigidBody>
    {
        private readonly IStorage<RigidBody> _storage;
        private readonly IContainer<Clock> _clockContainer;
        private readonly IContainer<Terrain> _terrainContainer;
        private readonly Momentum _initialMomentum;
        private readonly Vector _initialPosition;
        private readonly RigidBodyProperties _properties;

        public RigidBodyCreated(IStorage<RigidBody> storage, IContainer<Clock> clockContainer, IContainer<Terrain> terrainContainer, Vector initialPosition, Momentum initialMomentum, RigidBodyProperties properties)
            : base(storage)
        {
            _storage = storage;
            _clockContainer = clockContainer;
            _terrainContainer = terrainContainer;
            _initialPosition = initialPosition;
            _initialMomentum = initialMomentum;
            _properties = properties;
        }

        protected override RigidBody Create()
        {
            return new RigidBody(_storage, _clockContainer.Entity, _terrainContainer.Entity, _initialPosition, _initialMomentum, _properties);
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<RigidBody> id)
        {
            reciever.RigidBodyCreated(id, _initialPosition);
        }
    }
}