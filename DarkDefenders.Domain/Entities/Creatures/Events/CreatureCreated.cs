using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Entities.RigidBodies;
using DarkDefenders.Domain.Entities.Terrains;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Factories;
using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Creatures.Events
{
    internal class CreatureCreated : Created<Creature>
    {
        private readonly IStorage<Creature> _storage;
        private readonly ProjectileFactory _projectileFactory;
        private readonly IContainer<Clock> _clockContainer;
        private readonly IContainer<Terrain> _terrainContainer;
        private readonly CreatureProperties _properties;
        private readonly IContainer<RigidBody> _rigidBody;

        public CreatureCreated(IStorage<Creature> storage, ProjectileFactory projectileFactory, IContainer<Clock> clockContainer, IContainer<Terrain> terrainContainer, IContainer<RigidBody> rigidBody, CreatureProperties properties) 
            : base(storage)
        {
            _storage = storage;
            _projectileFactory = projectileFactory;
            _clockContainer = clockContainer;
            _terrainContainer = terrainContainer;
            _properties = properties;
            _rigidBody = rigidBody;
        }

        protected override Creature Create()
        {
            return new Creature(_storage, _projectileFactory, _clockContainer.Item, _terrainContainer.Item, _rigidBody.Item, _properties);
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<Creature> id)
        {
            reciever.CreatureCreated(id, _rigidBody.Item.Id);
        }
    }
}