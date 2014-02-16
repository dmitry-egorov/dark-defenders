using DarkDefenders.Domain.Model.Entities;
using DarkDefenders.Domain.Model.Events;
using DarkDefenders.Domain.Model.Other;

namespace DarkDefenders.Remote.Model.Internals.Entities
{
    internal class RemoteCreature: ICreatureEvents
    {
        private readonly RemoteEventAdapter _adapter;

        public RemoteCreature(RemoteEventAdapter adapter)
        {
            _adapter = adapter;
        }

        public void Created(Creature creature, RigidBody rigidBody, string properties)
        {
            _adapter.CreatureCreated(creature, rigidBody);
        }

        public void MovementChanged(Movement movement)
        {
        }

        public void Destroyed()
        {
        }
    }
}