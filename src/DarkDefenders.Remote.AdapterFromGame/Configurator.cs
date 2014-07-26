using DarkDefenders.Game.App;
using DarkDefenders.Game.Model.Events;
using DarkDefenders.Remote.AdapterFromGame.Internals;
using DarkDefenders.Remote.AdapterFromGame.Internals.Entities;

namespace DarkDefenders.Remote.AdapterFromGame
{
    public static class Configurator
    {
        public static GameBootstrapper RegisterRemoteEvents(this GameBootstrapper bootstrapper)
        {
            return bootstrapper
            .RegisterSingleton<RemoteState, IRemoteEventsSource>()
            .RegisterSingleton<RemoteEventsAdapter>()
            .RegisterListener<ITerrainEvents, TerrainAdapter>()
            .RegisterListener<IRigidBodyEvents, RigidBodyAdapter>()
            .RegisterListener<ICreatureEvents, CreatureAdapter>()
            .RegisterListener<IPlayerEvents, PlayerAdapter>()
            .RegisterListener<IHeroEvents, HeroAdapter>()
            .RegisterListener<IProjectileEvents, ProjectileAdapter>()
            ;
        }
    }
}