using System;
using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Entities.Heroes;
using DarkDefenders.Domain.Entities.Projectiles;
using DarkDefenders.Domain.Entities.RigidBodies;
using DarkDefenders.Domain.Entities.Terrains;
using DarkDefenders.Domain.Entities.Worlds;
using DarkDefenders.Domain.Interfaces;
using DarkDefenders.Dtos.Entities.Worlds;
using DarkDefenders.Dtos.Other;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Adapters
{
    internal class GameAdapter : IGame
    {
        private readonly IEventsProcessor _processor;

        private readonly FactoryAdapter<ClockFactory> _clockFactory;
        private readonly FactoryAdapter<TerrainFactory> _terrainFactory;
        private readonly FactoryAdapter<WorldFactory> _worldFactory;

        private readonly RootAdapter<World> _world;
        private readonly RootAdapter<Clock> _clock;
        private readonly RootsAdapter<RigidBody> _rigidBodies;
        private readonly RootsAdapter<Projectile> _projectiles;
        private readonly RootsAdapter<Hero> _heroes;

        public GameAdapter(IEventsProcessor processor, ClockFactory clockFactory, TerrainFactory terrainFactory, WorldFactory worldFactory, IContainer<World> worldContainer, IRepository<Hero> heroRepository, IRepository<RigidBody> rigidBodyRepository, IRepository<Projectile> projectileRepository, IContainer<Clock> clockContainer)
        {
            _processor = processor;
            _clockFactory = new FactoryAdapter<ClockFactory>(clockFactory, _processor);
            _terrainFactory = new FactoryAdapter<TerrainFactory>(terrainFactory, _processor);
            _worldFactory = new FactoryAdapter<WorldFactory>(worldFactory, _processor);

            _world = new RootAdapter<World>(worldContainer, _processor);
            _clock = new RootAdapter<Clock>(clockContainer, _processor);
            _rigidBodies = new RootsAdapter<RigidBody>(rigidBodyRepository, _processor);
            _projectiles = new RootsAdapter<Projectile>(projectileRepository, _processor);
            _heroes = new RootsAdapter<Hero>(heroRepository, _processor);
        }

        public IWorld Initialize(Map<Tile> map, WorldProperties worldProperties)
        {
            _clockFactory.Commit(x => x.Create());
            _terrainFactory.Commit(x => x.Create(map));
            _worldFactory.Commit(x => x.Create(worldProperties));

            return new WorldAdapter(_processor, _world);
        }

        public void Update(TimeSpan elapsed)
        {
            _clock.Commit(x => x.UpdateTime(elapsed));
            _rigidBodies.Commit(x => x.UpdatePhysics());
            _projectiles.Commit(x => x.CheckForHit());
            _world.Commit(x => x.SpawnHeroes());
            _heroes.Commit(x => x.Think());
        }

        public void KillAllHeroes()
        {
            _heroes.Commit(x => x.Kill());
        }
    }
}