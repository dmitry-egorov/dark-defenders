using DarkDefenders.Dtos.Infrastructure;

namespace DarkDefenders.Dtos.Entities.RigidBodies
{
    public class RigidBodyCreatedDto : EventDto<RigidBodyCreatedDto>
    {
        public RigidBodyId RigidBodyId { get; private set; }
        public RigidBodyInitialProperties Properties { get; private set; }

        public RigidBodyCreatedDto(RigidBodyId rigidBodyId, RigidBodyInitialProperties properties)
        {
            RigidBodyId = rigidBodyId;
            Properties = properties;
        }

        public override void Accept(IEventDtoReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}