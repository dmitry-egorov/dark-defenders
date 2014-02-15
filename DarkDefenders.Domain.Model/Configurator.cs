using System;
using DarkDefenders.Domain.Model.Entities.Clocks;
using DarkDefenders.Domain.Model.Entities.Creatures;
using DarkDefenders.Domain.Model.Entities.Heroes;
using DarkDefenders.Domain.Model.Entities.HeroSpawners;
using DarkDefenders.Domain.Model.Entities.HeroSpawnPoints;
using DarkDefenders.Domain.Model.Entities.Players;
using DarkDefenders.Domain.Model.Entities.PlayerSpawners;
using DarkDefenders.Domain.Model.Entities.Projectiles;
using DarkDefenders.Domain.Model.Entities.RigidBodies;
using DarkDefenders.Domain.Model.Entities.Terrains;
using DarkDefenders.Domain.Model.Entities.Weapons;
using DarkDefenders.Domain.Model.Entities.Worlds;
using DarkDefenders.Domain.Model.Other;
using Infrastructure.DDDES.Implementations;
using Infrastructure.Unity;
using Microsoft.Practices.Unity;

namespace DarkDefenders.Domain.Model
{
    public static class Configurator
    {
        public static IUnityContainer RegisterDomain(this IUnityContainer container)
        {
            return container
                .RegisterProcessor<IEventsReciever>()

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
        }
    }
}