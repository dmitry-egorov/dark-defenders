using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.Remote.Model
{
    public class RemoteEntity
    {
        public IdentityOf<RemoteEntity> Id { get; private set; }
        public Vector Position { get; set; }
        public RemoteEntityType Type { get; private set; }

        public RemoteEntity(IdentityOf<RemoteEntity> id, Vector initialPosition, RemoteEntityType type)
        {
            Id = id;
            Position = initialPosition;
            Type = type;
        }


    }
}