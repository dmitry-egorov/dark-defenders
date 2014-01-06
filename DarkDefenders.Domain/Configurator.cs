using DarkDefenders.Domain.Clocks;
using DarkDefenders.Domain.Clocks.Events;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Creatures.Events;
using DarkDefenders.Domain.Projectiles;
using DarkDefenders.Domain.Projectiles.Events;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.RigidBodies.Events;
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
            var clockRepository = new Repository<ClockId, Clock>();
            var worldRepository = new Repository<WorldId, World>();
            var rigidBodyRepository = new Repository<RigidBodyId, RigidBody>();
            var creatureRepository = new Repository<CreatureId, Creature>();
            var projectileRepository = new Repository<ProjectileId, Projectile>();

            var rigidBodyFactory = new RigidBodyFactory(rigidBodyRepository, clockRepository, worldRepository);
            var projectileFactory = new ProjectileFactory(projectileRepository, rigidBodyRepository, rigidBodyFactory);
            var creatureFactory = new CreatureFactory(creatureRepository, worldRepository, rigidBodyRepository, clockRepository, rigidBodyFactory, projectileFactory);
            var clockFactory = new ClockFactory(clockRepository);
            var worldFactory = new WorldFactory(worldRepository, clockRepository, creatureFactory);

            processor.RegisterRoot<ClockId, Clock, IClockEvent, ClockFactory, ClockCreated, ClockDestroyed>(clockRepository, clockFactory);
            processor.RegisterRoot<WorldId, World, IWorldEvent, WorldFactory, WorldCreated, WorldDestroyed>(worldRepository, worldFactory);
            processor.RegisterRoot<RigidBodyId, RigidBody, IRigidBodyEvent, RigidBodyFactory, RigidBodyCreated, RigidBodyDestroyed>(rigidBodyRepository, rigidBodyFactory);
            processor.RegisterRoot<CreatureId, Creature, ICreatureEvent, CreatureFactory, CreatureCreated, ClockDestroyed>(creatureRepository, creatureFactory);
            processor.RegisterRoot<ProjectileId, Projectile, IProjectileEvent, ProjectileFactory, ProjectileCreated, ProjectileDestroyed>(projectileRepository, projectileFactory);
        }
    }
}