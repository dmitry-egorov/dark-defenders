using DarkDefenders.Game.Model.Entities;
using DarkDefenders.Remote.Model;
using Infrastructure.DDDES;

namespace DarkDefenders.Remote.AdapterFromGame.Internals
{
    public static class GameExtensions
    {
        public static IdentityOf<RemoteRigidBody> ToRemote(this IdentityOf<RigidBody> id)
        {
            return new IdentityOf<RemoteRigidBody>(id.Value);
        }
    }
}