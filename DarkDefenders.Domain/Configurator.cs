using System;
using DarkDefenders.Domain.Clocks;
using DarkDefenders.Domain.Clocks.Events;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Creatures.Events;
using DarkDefenders.Domain.Heroes;
using DarkDefenders.Domain.Heroes.Events;
using DarkDefenders.Domain.Projectiles;
using DarkDefenders.Domain.Projectiles.Events;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.RigidBodies.Events;
using DarkDefenders.Domain.Terrains;
using DarkDefenders.Domain.Terrains.Events;
using DarkDefenders.Domain.Worlds;
using DarkDefenders.Domain.Worlds.Events;
using Infrastructure.DDDES.Implementations;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain
{
    public static class Configurator
    {
        public static void ConfigureDomain(this CommandProcessor<IDomainEvent> processor)
        {
            var random = new Random();

            var clockRepository = new Repository<ClockId, Clock>();
            var terrainRepository = new Repository<TerrainId, Terrain>();
            var worldRepository = new Repository<WorldId, World>();
            var rigidBodyRepository = new Repository<RigidBodyId, RigidBody>();
            var creatureRepository = new Repository<CreatureId, Creature>();
            var projectileRepository = new Repository<ProjectileId, Projectile>();
            var heroRepository = new Repository<HeroId, Hero>();

            var rigidBodyFactory = new RigidBodyFactory(rigidBodyRepository, clockRepository, terrainRepository);
            var projectileFactory = new ProjectileFactory(projectileRepository, rigidBodyRepository, rigidBodyFactory);
            var creatureFactory = new CreatureFactory(clockRepository, terrainRepository, creatureRepository, rigidBodyRepository, rigidBodyFactory, projectileFactory);
            var heroFactory = new HeroFactory(heroRepository, creatureRepository, creatureFactory, random);
            var clockFactory = new ClockFactory(clockRepository);
            var terrainFactory = new TerrainFactory(terrainRepository);
            var worldFactory = new WorldFactory(worldRepository, clockRepository, heroRepository, creatureFactory, heroFactory, random);

            processor.RegisterRoot<ClockId, Clock, IClockEvent, ClockFactory, ClockCreated, ClockDestroyed>(clockRepository, clockFactory);
            processor.RegisterRoot<TerrainId, Terrain, ITerrainEvent, TerrainFactory, TerrainCreated, TerrainDestroyed>(terrainRepository, terrainFactory);
            processor.RegisterRoot<WorldId, World, IWorldEvent, WorldFactory, WorldCreated, WorldDestroyed>(worldRepository, worldFactory);
            processor.RegisterRoot<RigidBodyId, RigidBody, IRigidBodyEvent, RigidBodyFactory, RigidBodyCreated, RigidBodyDestroyed>(rigidBodyRepository, rigidBodyFactory);
            processor.RegisterRoot<CreatureId, Creature, ICreatureEvent, CreatureFactory, CreatureCreated, CreatureDestroyed>(creatureRepository, creatureFactory);
            processor.RegisterRoot<HeroId, Hero, IHeroEvent, HeroFactory, HeroCreated, HeroDestroyed>(heroRepository, heroFactory);
            processor.RegisterRoot<ProjectileId, Projectile, IProjectileEvent, ProjectileFactory, ProjectileCreated, ProjectileDestroyed>(projectileRepository, projectileFactory);
        }
    }
}