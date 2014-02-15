using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DarkDefenders.Domain.Model.Entities.Players;
using DarkDefenders.Domain.Model.Entities.PlayerSpawners.Events;
using DarkDefenders.Domain.Model.Entities.Worlds;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities.PlayerSpawners
{
    [UsedImplicitly]
    public class PlayerSpawner : Entity<PlayerSpawner>
    {
        private readonly IStorage<PlayerSpawner> _storage;
        private readonly Random _random;
        private readonly IResources<WorldProperties> _resources;

        private ReadOnlyCollection<Vector> _playersSpawnPositions;

        public PlayerSpawner(IStorage<PlayerSpawner> storage, Random random, IResources<WorldProperties> resources)
        {
            _storage = storage;
            _random = random;
            _resources = resources;
        }

        public IEnumerable<IEvent> Create(string mapId)
        {
            yield return new PlayerSpawnerCreated(this, _storage, mapId);
        }

        public IEnumerable<IEvent> Spawn(Player player)
        {
            var position = _random.ElementFrom(_playersSpawnPositions);

            var events = player.Create(position);

            foreach (var e in events) { yield return e; }
        }

        internal void Created(string mapId)
        {
            _playersSpawnPositions = _resources[mapId].PlayersSpawnPositions;
        }
    }
}