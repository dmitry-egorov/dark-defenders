using DarkDefenders.Game.Model.Entities;
using DarkDefenders.Game.Model.Events;
using DarkDefenders.Game.Model.Other;
using DarkDefenders.Kernel.Model;

namespace DarkDefenders.Remote.AdapterFromGame.Internals.Entities
{
    internal class CreatureAdapter : ICreatureEvents
    {
        private readonly RemoteEventsAdapter _adapter;
        private RigidBody _rigidBody;

        public CreatureAdapter(RemoteEventsAdapter adapter)
        {
            _adapter = adapter;
        }

        public void Created(Creature creature, RigidBody rigidBody, string properties)
        {
            _rigidBody = rigidBody;
            _adapter.CreatureCreated(creature, rigidBody);
        }

        public void MovementChanged(Movement movement, Direction direction)
        {
            _adapter.ChangedDirection(_rigidBody, direction);
        }

        public void Destroyed()
        {
        }
    }
}