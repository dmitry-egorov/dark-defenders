using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Players.Events;
using DarkDefenders.Domain.Terrains;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Players
{
    public class Player : RootBase<PlayerSnapshot, PlayerId>, IUpdateable
    {
        private readonly IRepository<Terrain, TerrainId> _terrainRepository;
        private const double Speed = 0.001d;

        public Player(IRepository<Terrain, TerrainId> terrainRepository)
        {
            _terrainRepository = terrainRepository;
        }

        public IEnumerable<IPlayerEvent> Create(PlayerId id, TerrainId terrainId)
        {
            AssertDoesntExist();

            var terrain = _terrainRepository.GetById(terrainId);

            var spawnPosition = terrain.GetSpawnPosition();

            yield return new PlayerCreated(id, terrainId, spawnPosition);
        }

        public IEnumerable<IEvent> SetDesiredOrientation(Vector orientation)
        {
            AssertExists();

            if (Snapshot.DesiredOrientation != orientation)
            {
                yield return new PlayersDesiredOrientationIsSet(Snapshot.Id, orientation);
            }
        }

        public IEnumerable<IEvent> Update(TimeSpan elapsed)
        {
            var delta = Snapshot.DesiredOrientation * elapsed.TotalMilliseconds * Speed;

            if (delta.X == 0 && delta.Y == 0)
            {
                yield break;
            }

            var newPosition = Snapshot.Position + delta;

            var terrain = _terrainRepository.GetById(Snapshot.TerrainId);

            if (terrain.IsAllowedPosition(newPosition))
            {
                yield return new PlayerMoved(Snapshot.Id, newPosition);
            }
        }
    }
}
