using System;
using System.Collections.Generic;
using DarkDefenders.Remote.Model;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Remote.AdapterFromGame.Internals
{
    public class RemoteEventsPacker: IRemoteEvents
    {
        private readonly Queue<Action<IRemoteEvents>> _actionsQueue = new Queue<Action<IRemoteEvents>>();
        private readonly IEventsListener<IRemoteEvents> _listener;

        public RemoteEventsPacker(IEventsListener<IRemoteEvents> listener)
        {
            _listener = listener;
        }

        public void MapLoaded(string mapId)
        {
            _actionsQueue.Enqueue(r => r.MapLoaded(mapId));
        }

        public void Created(IdentityOf<RemoteRigidBody> id, Vector initialPosition, RemoteEntityType type)
        {
            _actionsQueue.Enqueue(r => r.Created(id, initialPosition, type));
        }

        public void Moved(IdentityOf<RemoteRigidBody> id, Vector newPosition)
        {
            _actionsQueue.Enqueue(r => r.Moved(id, newPosition));
        }

        public void Destroyed(IdentityOf<RemoteRigidBody> id)
        {
            _actionsQueue.Enqueue(r => r.Destroyed(id));
        }

        public void Pack()
        {
            var actions = _actionsQueue.DequeueAll().AsReadOnly();

            _listener.Recieve(actions);
        }
    }
}