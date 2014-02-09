using System;
using DarkDefenders.Domain.Adapters;
using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.Heroes;
using DarkDefenders.Domain.Entities.Projectiles;
using DarkDefenders.Domain.Entities.RigidBodies;
using DarkDefenders.Domain.Entities.Terrains;
using DarkDefenders.Domain.Entities.Worlds;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Factories;
using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Unity;
using Microsoft.Practices.Unity;

namespace DarkDefenders.Domain
{
    public static class Configurator
    {
        public static void RegisterDomain(this IUnityContainer container)
        {
            container
            .RegisterProcessor()

            .RegisterRepositoryFor<RigidBody>()
            .RegisterRepositoryFor<Creature>()
            .RegisterRepositoryFor<Projectile>()
            .RegisterRepositoryFor<Hero>()
            .RegisterContainerFor<Terrain>()
            .RegisterContainerFor<Clock>()
            .RegisterContainerFor<World>()
            .RegisterSingleton<RigidBodyFactory>()
            .RegisterSingleton<ProjectileFactory>()
            .RegisterSingleton<CreatureFactory>()
            .RegisterSingleton<HeroFactory>()
            .RegisterSingleton<WorldFactory>()
            .RegisterSingleton<ClockFactory>()
            .RegisterSingleton<Random>(new InjectionConstructor())
            ;
        }

        public static IGame ResolveGame(this IUnityContainer container)
        {
            return container.Resolve<GameAdapter>();
        }

        private static IUnityContainer RegisterProcessor(this IUnityContainer container)
        {
            return container.RegisterSingleton<EventsProcessor<IEventsReciever>, IEventsProcessor>();
        }

        private static IUnityContainer RegisterContainerFor<T>(this IUnityContainer container) 
            where T : class
        {
            return container
                .RegisterSingleton<Container<T>, IStorage<T>, IContainer<T>>();
        }

        private static IUnityContainer RegisterRepositoryFor<T>(this IUnityContainer container)
        {
            return container
                .RegisterSingleton<Repository<T>, IStorage<T>, IRepository<T>>();
        }
    }
}