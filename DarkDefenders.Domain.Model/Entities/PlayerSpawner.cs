using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DarkDefenders.Domain.Model.EntityProperties;
using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities
{
    [UsedImplicitly]
    public class PlayerSpawner : Entity<PlayerSpawner, IPlayerSpawnerEvents>, IPlayerSpawnerEvents
    {
        private readonly Random _random;
        private readonly IResources<WorldProperties> _resources;

        private ReadOnlyCollection<Vector> _playersSpawnPositions;

        public PlayerSpawner(IPlayerSpawnerEvents external, IStorage<PlayerSpawner> storage, Random random, IResources<WorldProperties> resources) 
            : base(external, storage)
        {
            _random = random;
            _resources = resources;
        }

        public IEnumerable<IEvent> Create(string mapId)
        {
            yield return CreationEvent(x => x.Created(mapId));
        }

        public IEnumerable<IEvent> Spawn(Player player)
        {
            var position = _random.ElementFrom(_playersSpawnPositions);

            var events = player.Create(position);

            foreach (var e in events) { yield return e; }
        }

        void IPlayerSpawnerEvents.Created(string mapId)
        {
            _playersSpawnPositions = _resources[mapId].PlayersSpawnPositions;
        }

        void IEntityEvents.Destroyed()
        {
        }
    }
}