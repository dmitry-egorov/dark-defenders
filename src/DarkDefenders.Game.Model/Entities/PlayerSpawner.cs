using System;
using System.Collections.ObjectModel;
using DarkDefenders.Game.Model.EntityProperties;
using DarkDefenders.Game.Model.Events;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;
using JetBrains.Annotations;

namespace DarkDefenders.Game.Model.Entities
{
    [UsedImplicitly]
    public class PlayerSpawner : Entity<PlayerSpawner, IPlayerSpawnerEvents>, IPlayerSpawnerEvents
    {
        private readonly Random _random;
        private readonly IResources<WorldProperties> _resources;

        private ReadOnlyCollection<Vector> _playersSpawnPositions;
        private readonly IFactory<Player> _playerFactory;

        public PlayerSpawner
        (
            Random random, 
            IResources<WorldProperties> resources, 
            IFactory<Player> playerFactory
        )
        {
            _random = random;
            _resources = resources;
            _playerFactory = playerFactory;
        }

        public void Create(string mapId)
        {
            CreationEvent(x => x.Created(mapId));
        }

        public Player Spawn()
        {
            var player = _playerFactory.Create();

            var position = _random.ElementFrom(_playersSpawnPositions);

            player.Create(position);

            return player;
        }

        void IPlayerSpawnerEvents.Created(string mapId)
        {
            _playersSpawnPositions = _resources[mapId].PlayersSpawnPositions;
        }
    }
}