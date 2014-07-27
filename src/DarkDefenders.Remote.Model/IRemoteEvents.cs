using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.Remote.Model
{
    public interface IRemoteEvents
    {
        void MapLoaded(string mapId);
        void Created(IdentityOf<RemoteEntity> id, Vector initialPosition, RemoteEntityType type);
        void Moved(IdentityOf<RemoteEntity> id, Vector newPosition);
        void ChangedDirection(IdentityOf<RemoteEntity> id, HorizontalDirection newDirection);
        void Destroyed(IdentityOf<RemoteEntity> id);
    }
}