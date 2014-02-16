using DarkDefenders.Domain.Model.Entities;
using DarkDefenders.Domain.Model.Events;
using DarkDefenders.Domain.Model.Other;
using Infrastructure.DDDES;

namespace DarkDefenders.Remote.Model.Internals.Entities
{
    internal class RemoteCreature: ICreatureEvents
    {
        private readonly RemoteEventAdapter _adapter;

        public RemoteCreature(RemoteEventAdapter adapter)
        {
            _adapter = adapter;
        }

        public void Created(IdentityOf<Creature> creatureId, IdentityOf<RigidBody> rigidBodyId, string properties)
        {
            _adapter.CreatureCreated(creatureId, rigidBodyId);
        }

        public void MovementChanged(Movement movement)
        {
        }

        public void Destroyed()
        {
        }
    }
}