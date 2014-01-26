using DarkDefenders.Dtos.Infrastructure;
using Infrastructure.Math;

namespace DarkDefenders.Dtos.Entities.RigidBodies
{
    public class MovedDto : EventDto<MovedDto>
    {
        public RigidBodyId RigidBodyId { get; private set; }
        public Vector NewPosition { get; private set; }

        public MovedDto(RigidBodyId rigidBodyId, Vector newPosition)
        {
            RigidBodyId = rigidBodyId;
            NewPosition = newPosition;
        }

        public override void Accept(IEventDtoReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}