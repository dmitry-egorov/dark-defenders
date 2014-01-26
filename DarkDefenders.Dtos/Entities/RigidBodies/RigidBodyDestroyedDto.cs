using DarkDefenders.Dtos.Infrastructure;

namespace DarkDefenders.Dtos.Entities.RigidBodies
{
    public class RigidBodyDestroyedDto : EventDto<RigidBodyDestroyedDto>
    {
        public RigidBodyId RigidBodyId { get; private set; }

        public RigidBodyDestroyedDto(RigidBodyId rigidBodyId)
        {
            RigidBodyId = rigidBodyId;
        }

        public override void Accept(IEventDtoReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}