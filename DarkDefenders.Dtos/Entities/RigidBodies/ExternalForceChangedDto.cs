using DarkDefenders.Dtos.Infrastructure;
using Infrastructure.Physics;

namespace DarkDefenders.Dtos.Entities.RigidBodies
{
    public class ExternalForceChangedDto : EventDto<ExternalForceChangedDto>
    {
        public RigidBodyId RigidBodyId { get; private set; }
        public Force ExternalForce { get; private set; }

        public ExternalForceChangedDto(RigidBodyId rigidBodyId, Force externalForce)
        {
            RigidBodyId = rigidBodyId;
            ExternalForce = externalForce;
        }

        public override void Accept(IEventDtoReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}