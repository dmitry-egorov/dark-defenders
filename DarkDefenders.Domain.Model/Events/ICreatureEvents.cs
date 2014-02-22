using DarkDefenders.Domain.Model.Entities;
using DarkDefenders.Domain.Model.Other;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Events
{
    public interface ICreatureEvents : IEntityEvents
    {
        void Created(Creature creature, RigidBody rigidBody, string properties);
        void MovementChanged(Movement movement);
    }
}