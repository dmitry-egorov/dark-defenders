using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Model.Entities;
using DarkDefenders.Remote.Model.Interface;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Remote.Model.Internals
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

        public void Created(IdentityOf<RigidBody> id, Vector initialPosition, RemoteEntityType type)
        {
            _actionsQueue.Enqueue(r => r.Created(id, initialPosition, type));
        }

        public void Moved(IdentityOf<RigidBody> id, Vector newPosition)
        {
            _actionsQueue.Enqueue(r => r.Moved(id, newPosition));
        }

        public void Destroyed(IdentityOf<RigidBody> id)
        {
            _actionsQueue.Enqueue(r => r.Destroyed(id));
        }

        public void Tick(TimeSpan newTime)
        {
            var actions = _actionsQueue.DequeueAll();

            _listener.Recieve(actions);
        }
    }
}