using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.Remote.Model
{
    public interface IRemoteEvents
    {
        void MapLoaded(string mapId);
        void Created(IdentityOf<RemoteRigidBody> id, Vector initialPosition, RemoteEntityType type);
        void Moved(IdentityOf<RemoteRigidBody> id, Vector newPosition);
        void Destroyed(IdentityOf<RemoteRigidBody> id);
    }
}