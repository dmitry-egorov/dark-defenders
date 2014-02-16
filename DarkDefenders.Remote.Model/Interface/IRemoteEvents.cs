using System;
using DarkDefenders.Domain.Model.Entities;
using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.Remote.Model.Interface
{
    public interface IRemoteEvents
    {
        void MapLoaded(string mapId);
        void Created(IdentityOf<RigidBody> id, Vector initialPosition, RemoteEntityType type);
        void Moved(IdentityOf<RigidBody> id, Vector newPosition);
        void Destroyed(IdentityOf<RigidBody> id);
        void Tick(TimeSpan newTime);
    }
}