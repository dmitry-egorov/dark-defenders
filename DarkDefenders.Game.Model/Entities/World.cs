using System;
using System.Collections.Generic;
using DarkDefenders.Game.Model.Events;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using JetBrains.Annotations;

namespace DarkDefenders.Game.Model.Entities
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
            Terrain terrain,
            PlayerSpawner playerSpawner,
            HeroSpawner heroSpawner,
            Clock clock,
            IReadOnlyList<Hero> heroes, 
            IReadOnlyList<RigidBody> rigidBodies,
            IReadOnlyList<Projectile> projectiles
        ) 
        {
            _terrain = terrain;
            _playerSpawner = playerSpawner;
            _heroSpawner = heroSpawner;
            _heroes = heroes;
            _rigidBodies = rigidBodies;
            _projectiles = projectiles;
            _clock = clock;
        }

        public void Create(string mapId)
        {
            _clock.Create();
            _terrain.Create(mapId);
            _playerSpawner.Create(mapId);
            _heroSpawner.Create(mapId);

            CreationEvent(x => x.Created());
        }

        public void Update(TimeSpan elapsed)
        {
            _heroes.ForAll(x => x.Think());
            _heroSpawner.Update();
            _projectiles.ForAll(x => x.CheckForHit());
            _rigidBodies.ForAll(x => x.UpdatePhysics(elapsed));
            _clock.UpdateTime(elapsed);
        }

        public void KillAllHeroes()
        {
            _heroes.ForAll(x => x.Kill());
        }

        void IWorldEvents.Created()
        {
        }
    }
}
