using DarkDefenders.Dtos.Infrastructure;
using Infrastructure.Math;
using Infrastructure.Physics;

namespace DarkDefenders.Dtos.Entities.RigidBodies
{
    public class AcceleratedAndMovedDto : EventDto<AcceleratedAndMovedDto>
    {
        public RigidBodyId RigidBodyId { get; private set; }
        public Vector NewPosition { get; private set; }
        public Momentum NewMomentum { get; private set; }

        public AcceleratedAndMovedDto(RigidBodyId rigidBodyId, Vector newPosition, Momentum newMomentum)
        {
            RigidBodyId = rigidBodyId;
            NewPosition = newPosition;
            NewMomentum = newMomentum;
        }

        public override void Accept(IEventDtoReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}