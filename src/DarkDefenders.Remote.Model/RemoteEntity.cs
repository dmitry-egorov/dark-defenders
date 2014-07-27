using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.Remote.Model
{
    public class RemoteEntity
    {
        public IdentityOf<RemoteEntity> Id { get; private set; }
        public Vector Position { get; set; }
        public HorizontalDirection Direction { get; set; }
        public RemoteEntityType Type { get; private set; }

        public RemoteEntity(IdentityOf<RemoteEntity> id, Vector initialPosition, HorizontalDirection initialDirection, RemoteEntityType type)
        {
            Id = id;
            Position = initialPosition;
            Direction = initialDirection;
            Type = type;
        }


    }
}