using System;
using System.Collections.Generic;
using DarkDefenders.Kernel.Model;
using DarkDefenders.Remote.Model;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Infrastructure.Util;
using JetBrains.Annotations;

namespace DarkDefenders.Remote.AdapterFromGame.Internals
{
    [UsedImplicitly]
    internal class RemoteState : IRemoteEventsSource
    {
        private readonly Queue<Action<IRemoteEvents>> _actionsQueue = new Queue<Action<IRemoteEvents>>();

        private readonly Dictionary<IdentityOf<RemoteEntity>, RemoteEntity> _currentEntities = new Dictionary<IdentityOf<RemoteEntity>, RemoteEntity>();
        private string _currentMapId;

        public void MapLoaded(string mapId)
        {
            _currentMapId = mapId;

            _actionsQueue.Enqueue(r => r.MapLoaded(mapId));
        }

        public void Created(IdentityOf<RemoteEntity> id, Vector initialPosition, Direction initialDirection, RemoteEntityType type)
        {
            _currentEntities.Add(id, new RemoteEntity(id, initialPosition, initialDirection, type));

            _actionsQueue.Enqueue(r => r.Created(id, initialPosition, type));
        }

        public void Moved(IdentityOf<RemoteEntity> id, Vector newPosition)
        {
            _currentEntities[id].Position = newPosition;

            _actionsQueue.Enqueue(r => r.Moved(id, newPosition));
        }

        public void ChangedDirection(IdentityOf<RemoteEntity> id, Direction newDirection)
        {
            _currentEntities[id].Direction = newDirection;

            _actionsQueue.Enqueue(r => r.ChangedDirection(id, newDirection));
        }

        public void Destroyed(IdentityOf<RemoteEntity> id)
        {
            _currentEntities.Remove(id);

            _actionsQueue.Enqueue(r => r.Destroyed(id));
        }

        public IEnumerable<Action<IRemoteEvents>> GetEvents()
        {
            return _actionsQueue.DequeueAll().AsReadOnly();
        }

        public IEnumerable<Action<IRemoteEvents>> GetCurrentStateEvents()
        {
            yield return r => r.MapLoaded(_currentMapId);
            foreach (var entity in _currentEntities.Values)
            {
                var copy = entity;
                yield return r => r.Created(copy.Id, copy.Position, copy.Type);
            }
        }
    }
}