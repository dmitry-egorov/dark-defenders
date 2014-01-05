using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Worlds.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Math.Physics;

namespace DarkDefenders.Domain.Worlds
{
    public class World : RootBase<WorldId, IWorldEventsReciever, IWorldEvent>, IWorldEventsReciever
    {
        public IEnumerable<IWorldEvent> UpdateWorldTime(Seconds elapsed)
        {
            var newTime = _currentTime + elapsed;

            yield return new WorldTimeUpdated(Id, newTime, elapsed);
        }

        public IEnumerable<IDomainEvent> SpawnPlayer(CreatureId creatureId)
        {
            var events = _creatureFactory.Create(creatureId, Id, _playersSpawnPosition, _playersRigidBodyProperties);

            foreach (var e in events) { yield return e; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Seconds GetCurrentTime()
        {
            return _currentTime;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Seconds GetElapsed()
        {
            return _elapsed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AnySolidWallsAt(Axis axis, int mainStart, int mainEnd, int other)
        {
            return _terrain.IsAnyAtLine(axis, mainStart, mainEnd, other, Tile.Solid);
        }

        public void Recieve(WorldTimeUpdated worldTimeUpdated)
        {
            _currentTime = worldTimeUpdated.NewTime;
            _elapsed = worldTimeUpdated.Elapsed;
        }

        internal World(WorldId id, Map<Tile> terrain, Vector playersSpawnPosition, RigidBodyProperties playersRigidBodyProperties, CreatureFactory creatureFactory) : base(id)
        {
            _creatureFactory = creatureFactory;
            _playersRigidBodyProperties = playersRigidBodyProperties;

            _terrain = terrain;
            _playersSpawnPosition = playersSpawnPosition;
            _currentTime = Seconds.Zero;
        }


        private readonly CreatureFactory _creatureFactory;

        private readonly Vector _playersSpawnPosition;
        private readonly RigidBodyProperties _playersRigidBodyProperties;
        private readonly Map<Tile> _terrain;
        private Seconds _elapsed;
        private Seconds _currentTime;
    }
}
