using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Model.Entities;
using DarkDefenders.Domain.Model.Other;
using Infrastructure.DDDES.Implementations.Configuration;
using Microsoft.Practices.Unity;

namespace DarkDefenders.Domain.Model
{
    public static class Configurator
    {
        public static IUnityContainer RegisterDomain(this IUnityContainer container, IEnumerable<Type> registeredListeners)
        {
            new DomainConfigurator(container, registeredListeners)

            .RegisterEntity<RigidBody>()
            .RegisterEntity<Creature>()
            .RegisterEntity<Weapon>()
            .RegisterEntity<Projectile>()
            .RegisterEntity<Player>()
            .RegisterEntity<Hero>()
            .RegisterEntity<HeroSpawnPoint>()
            .RegisterSingletonEntity<HeroSpawner>()
            .RegisterSingletonEntity<PlayerSpawner>()
            .RegisterSingletonEntity<Terrain>()
            .RegisterSingletonEntity<Clock>()
            .RegisterSingletonEntity<World>()

            .RegisterSingleton<CooldownFactory>()
            .RegisterSingleton<Random>(new InjectionConstructor())
            ;

            return container;
        }
    }
}