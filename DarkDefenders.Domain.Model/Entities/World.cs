using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities
{
    [UsedImplicitly]
    public class World : Entity<World, IWorldEvents>, IWorldEvents
    {
        private readonly Clock _clock;
        private readonly Terrain _terrain;
        private readonly PlayerSpawner _playerSpawner;
        private readonly HeroSpawner _heroSpawner;

        private readonly IReadOnlyList<Hero> _heroes;
        private readonly IReadOnlyList<RigidBody> _rigidBodies;
        private readonly IReadOnlyList<Projectile> _projectiles;

        public World
        (
            IWorldEvents external,
            IStorage<World> storage,
            Terrain terrain,
            PlayerSpawner playerSpawner,
            HeroSpawner heroSpawner,
            Clock clock,
            IReadOnlyList<Hero> heroes, 
            IReadOnlyList<RigidBody> rigidBodies,
            IReadOnlyList<Projectile> projectiles
        ) 
        : base(external, storage)
        {
            _terrain = terrain;
            _playerSpawner = playerSpawner;
            _heroSpawner = heroSpawner;
            _heroes = heroes;
            _rigidBodies = rigidBodies;
            _projectiles = projectiles;
            _clock = clock;
        }

        public IEnumerable<IEvent> Create(string mapId)
        {
            var cevents = _clock.Create();

            var tevents = _terrain.Create(mapId);

            var psevents = _playerSpawner.Create(mapId);

            var hspevents = _heroSpawner.Create(mapId);

            var events = Concat.All(cevents, tevents, psevents, hspevents);
            foreach (var e in events) { yield return e; }

            yield return CreationEvent(x => x.Created(mapId));
        }

        public IEnumerable<IEvent> Update(TimeSpan elapsed)
        {
            var hevents = _heroes.ForAll(x => x.Think());
            var wevents = _heroSpawner.Update();
            var pevents = _projectiles.ForAll(x => x.CheckForHit());
            var revents = _rigidBodies.ForAll(x => x.UpdatePhysics());
            var cevents = _clock.UpdateTime(elapsed);

            var events = Concat.All(hevents, wevents, pevents, revents, cevents);

            foreach (var e in events) { yield return e; }
        }

        public IEnumerable<IEvent> KillAllHeroes()
        {
            var events = _heroes.ForAll(x => x.Kill());

            foreach (var e in events) { yield return e;}
        }

        void IWorldEvents.Created(string mapId)
        {
        }

        void IEntityEvents.Destroyed()
        {
        }
    }
}
