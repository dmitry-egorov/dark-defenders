using System;
using System.Collections.Generic;
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
using Infrastructure.Util;
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

        private readonly IReadOnlyCollection<RigidBody> _rigidBodies;
        private readonly IReadOnlyCollection<Projectile> _projectiles;
        private readonly IReadOnlyCollection<Hero> _heroes;

        private World _world;
        private Clock _clock;

        public GameAdapter
        (
            EventsProcessor<IEventsReciever> processor, 
            ClockFactory clockFactory, 
            TerrainFactory terrainFactory, 
            WorldFactory worldFactory, 
            IReadOnlyCollection<Hero> heroes,
            IReadOnlyCollection<RigidBody> rigidBodies, 
            IReadOnlyCollection<Projectile> projectiles
        )
        {
            _processor = processor;

            _clockFactory = new FactoryAdapter<Clock, ClockFactory>(clockFactory, _processor);
            _terrainFactory = new FactoryAdapter<Terrain, TerrainFactory>(terrainFactory, _processor);
            _worldFactory = new FactoryAdapter<World, WorldFactory>(worldFactory, _processor);

            _rigidBodies = rigidBodies;
            _projectiles = projectiles;
            _heroes = heroes;
        }

        public IWorld Initialize(string mapId, Map<Tile> map, WorldProperties worldProperties)
        {
            _clock = _clockFactory.Commit(x => x.Create());
            _terrainFactory.Commit(x => x.Create(map, mapId));
            _world = _worldFactory.Commit(x => x.Create(worldProperties));

            return new WorldAdapter(new EntityAdapter<World>(_world, _processor));
        }

        public void Update(TimeSpan elapsed)
        {
            var hevents = _heroes.ForAll(x => x.Think());
            var wevents = _world.SpawnHeroes();
            var pevents = _projectiles.ForAll(x => x.CheckForHit());
            var revents = _rigidBodies.ForAll(x => x.UpdatePhysics());
            var cevents = _clock.UpdateTime(elapsed);

            var events = Concat.All(hevents, wevents, pevents, revents, cevents);

            _processor.Process(events);
            _processor.Broadcast();
        }

        public void KillAllHeroes()
        {
            _processor.Process(_heroes, x => x.Kill());
        }
    }
}