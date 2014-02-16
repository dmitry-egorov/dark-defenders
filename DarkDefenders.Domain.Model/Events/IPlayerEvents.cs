using DarkDefenders.Domain.Model.Entities;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Events
{
    public interface IPlayerEvents: IEntityEvents
    {
        void Created(IdentityOf<Creature> creatureId);
    }
}