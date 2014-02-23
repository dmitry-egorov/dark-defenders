using DarkDefenders.Game.Model.Entities;
using DarkDefenders.Game.Model.Events;
using DarkDefenders.Game.Model.Other;

namespace DarkDefenders.Remote.AdapterFromGame.Internals.Entities
{
    internal class CreatureAdapter : ICreatureEvents
    {
        private readonly RemoteEventsAdapter _adapter;

        public CreatureAdapter(RemoteEventsAdapter adapter)
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