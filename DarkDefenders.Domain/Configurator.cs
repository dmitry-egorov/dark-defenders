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
            var worldRepository = new Repository<WorldId, World>();
            var rigidBodyRepository = new Repository<RigidBodyId, RigidBody>();
            var creatureRepository = new Repository<CreatureId, Creature>();
            var projectileRepository = new Repository<ProjectileId, Projectile>();

            var rigidBodyFactory = new RigidBodyFactory(rigidBodyRepository, worldRepository);
            var worldFactory = new WorldFactory(worldRepository);
            var projectileFactory = new ProjectileFactory(projectileRepository, rigidBodyRepository, rigidBodyFactory);
            var creatureFactory = new CreatureFactory(creatureRepository, worldRepository, rigidBodyRepository, rigidBodyFactory, projectileFactory);

            processor.RegisterRoot<WorldId, World, IWorldEvent, WorldFactory, WorldCreated>(worldRepository, worldFactory);
            processor.RegisterRoot<RigidBodyId, RigidBody, IRigidBodyEvent, RigidBodyFactory, RigidBodyCreated>(rigidBodyRepository, rigidBodyFactory);
            processor.RegisterRoot<CreatureId, Creature, ICreatureEvent, CreatureFactory, CreatureCreated>(creatureRepository, creatureFactory);
            processor.RegisterRoot<ProjectileId, Projectile, IProjectileEvent, ProjectileFactory, ProjectileCreated>(projectileRepository, projectileFactory);
        }
    }
}