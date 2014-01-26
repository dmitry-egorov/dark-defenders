using DarkDefenders.Dtos.Entities.RigidBodies;
using DarkDefenders.Dtos.Infrastructure;

namespace DarkDefenders.Dtos.Entities.Creatures
{
    public class CreatureCreatedDto : EventDto<CreatureCreatedDto>
    {
        public CreatureId CreatureId { get; private set; }
        public RigidBodyId RigidBodyId { get; private set; }
        public CreatureProperties Properties { get; private set; }

        public CreatureCreatedDto(CreatureId creatureId, RigidBodyId rigidBodyId, CreatureProperties properties)
        {
            CreatureId = creatureId;
            RigidBodyId = rigidBodyId;
            Properties = properties;
        }

        public override void Accept(IEventDtoReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}