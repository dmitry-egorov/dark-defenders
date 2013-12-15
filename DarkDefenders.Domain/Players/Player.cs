using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Players.Events;
using DarkDefenders.Domain.Terrains;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Players
{
    public class Player : RootBase<PlayerId, PlayerSnapshot, IPlayerEventsReciever, IPlayerEvent>, IUpdateable
    {
        private readonly IRepository<TerrainId, Terrain> _terrainRepository;
        private const double Speed = 0.001d;

        public Player(PlayerId id, IRepository<TerrainId, Terrain> terrainRepository) : base(id)
        {
            _terrainRepository = terrainRepository;
        }

        public IEnumerable<IPlayerEvent> Create(TerrainId terrainId)
        {
            AssertDoesntExist();

            var terrain = _terrainRepository.GetById(terrainId);

            var spawnPosition = terrain.GetSpawnPosition();

            yield return new PlayerCreated(Id, terrainId, spawnPosition);
        }

        public IEnumerable<IEvent> Stop()
        {
            return SetDesiredOrientation(Vector.Zero);
        }

        public IEnumerable<IEvent> Move(MoveDirection direction)
        {
            var orientation = direction == MoveDirection.Left ? Vector.Left : Vector.Right;

            return SetDesiredOrientation(orientation);
        }

        public IEnumerable<IEvent> Update(TimeSpan elapsed)
        {
            var snapshot = Snapshot;

            var delta = snapshot.DesiredOrientation * elapsed.TotalMilliseconds * Speed;

            if (delta == Vector.Zero)
            {
                yield break;
            }

            var newPosition = snapshot.Position + delta;

            var terrain = _terrainRepository.GetById(snapshot.TerrainId);

            if (terrain.IsAllowedPosition(newPosition))
            {
                yield return new PlayerMoved(Id, newPosition);
            }
        }

        private IEnumerable<IEvent> SetDesiredOrientation(Vector orientation)
        {
            if (Snapshot.DesiredOrientation != orientation)
            {
                yield return new PlayersDesiredOrientationIsSet(Id, orientation);
            }
        }
    }
}
