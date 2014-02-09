using System;
using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Entities.Heroes;
using DarkDefenders.Domain.Entities.Projectiles;
using DarkDefenders.Domain.Entities.RigidBodies;
using DarkDefenders.Domain.Entities.Terrains;
using DarkDefenders.Domain.Entities.Worlds;
using DarkDefenders.Domain.Factories;
using DarkDefenders.Domain.Interfaces;
using DarkDefenders.Domain.Other;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Adapters
{
    [UsedImplicitly]
    internal class GameAdapter : IGame
    {
        private readonly EventsProcessor<IEventsReciever> _processor;

        private readonly FactoryAdapter<Clock, ClockFactory> _clockFactory;
        private readonly FactoryAdapter<Terrain, TerrainFactory> _terrainFactory;
        private readonly FactoryAdapter<World, WorldFactory> _worldFactory;

        private readonly EntitiesAdapter<RigidBody> _rigidBodies;
        private readonly EntitiesAdapter<Projectile> _projectiles;
        private readonly EntitiesAdapter<Hero> _heroes;

        private EntityAdapter<World> _world;
        private EntityAdapter<Clock> _clock;

        public GameAdapter
        (
            EventsProcessor<IEventsReciever> processor, 
            ClockFactory clockFactory, 
            TerrainFactory terrainFactory, 
            WorldFactory worldFactory, 
            IRepository<Hero> heroRepository, 
            IRepository<RigidBody> rigidBodyRepository, 
            IRepository<Projectile> projectileRepository
        )
        {
            _processor = processor;

            _clockFactory = new FactoryAdapter<Clock, ClockFactory>(clockFactory, _processor);
            _terrainFactory = new FactoryAdapter<Terrain, TerrainFactory>(terrainFactory, _processor);
            _worldFactory = new FactoryAdapter<World, WorldFactory>(worldFactory, _processor);

            _rigidBodies = new EntitiesAdapter<RigidBody>(rigidBodyRepository, _processor);
            _projectiles = new EntitiesAdapter<Projectile>(projectileRepository, _processor);
            _heroes = new EntitiesAdapter<Hero>(heroRepository, _processor);
        }


        public IWorld Initialize(string mapId, Map<Tile> map, WorldProperties worldProperties)
        {
            _clock = _clockFactory.Commit(x => x.Create());
            _terrainFactory.Commit(x => x.Create(map, mapId));
            _world = _worldFactory.Commit(x => x.Create(worldProperties));

            return new WorldAdapter(_world);
        }
        public void Update(TimeSpan elapsed)
        {
            _clock.Commit(x => x.UpdateTime(elapsed));
            _rigidBodies.Commit(x => x.UpdatePhysics());
            _projectiles.Commit(x => x.CheckForHit());
            _world.Commit(x => x.SpawnHeroes());
            _heroes.Commit(x => x.Think());

            _processor.Broadcast();
        }

        public void KillAllHeroes()
        {
            _heroes.Commit(x => x.Kill());
        }
    }
}