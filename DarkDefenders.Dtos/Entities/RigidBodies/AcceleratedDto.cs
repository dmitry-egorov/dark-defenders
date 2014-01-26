using DarkDefenders.Dtos.Infrastructure;
using Infrastructure.Physics;

namespace DarkDefenders.Dtos.Entities.RigidBodies
{
    public class AcceleratedDto : EventDto<AcceleratedDto>
    {
        public RigidBodyId RigidBodyId { get; private set; }
        public Momentum NewMomentum { get; private set; }

        public AcceleratedDto(RigidBodyId rigidBodyId, Momentum newMomentum)
        {
            RigidBodyId = rigidBodyId;
            NewMomentum = newMomentum;
        }

        public override void Accept(IEventDtoReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}