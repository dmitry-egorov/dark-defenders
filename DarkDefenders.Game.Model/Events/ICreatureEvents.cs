using DarkDefenders.Game.Model.Entities;
using DarkDefenders.Game.Model.Other;
using Infrastructure.DDDES;

namespace DarkDefenders.Game.Model.Events
{
    public interface ICreatureEvents : IEntityEvents
    {
        void Created(Creature creature, RigidBody rigidBody, string properties);
        void MovementChanged(Movement movement);
    }
}