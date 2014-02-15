using DarkDefenders.Domain.Model.Entities.RigidBodies;
using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Entities.Creatures.Events
{
    internal class CreatureCreated : Created<Creature>
    {
        private readonly Creature _creature;
        private readonly CreatureProperties _properties;
        private readonly RigidBody _rigidBody;

        public CreatureCreated(Creature creature, IStorage<Creature> storage, RigidBody rigidBody, CreatureProperties properties) 
            : base(creature, storage)
        {
            _creature = creature;
            _properties = properties;
            _rigidBody = rigidBody;
        }


        protected override void ApplyTo(Creature creature)
        {
            creature.Created(_rigidBody, _properties);
        }

        public override void Accept(IEventsReciever reciever)
        {
            reciever.CreatureCreated(_creature.Id, _rigidBody.Id);
        }
    }
}