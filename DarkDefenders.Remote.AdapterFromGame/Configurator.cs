using DarkDefenders.Game.App;
using DarkDefenders.Game.Model.Events;
using DarkDefenders.Remote.AdapterFromGame.Internals;
using DarkDefenders.Remote.AdapterFromGame.Internals.Entities;
using DarkDefenders.Remote.Model;
using Infrastructure.DDDES;

namespace DarkDefenders.Remote.AdapterFromGame
{
    public static class Configurator
    {
        public static GameBootstrapper RegisterRemoteEvents(this GameBootstrapper bootstrapper, IEventsListener<IRemoteEvents> listener)
        {
            var packer = new RemoteEventsPacker(listener);
            var adapter = new RemoteEventAdapter(packer);

            return bootstrapper
            .RegisterListener<IClockEvents>     (() => new ClockAdapter(packer))
            .RegisterListener<ITerrainEvents>   (() => new TerrainAdapter(packer))
            .RegisterListener<IRigidBodyEvents> (() => new RigidBodyAdapter(adapter))
            .RegisterListener<ICreatureEvents>  (() => new CreatureAdapter(adapter))
            .RegisterListener<IPlayerEvents>    (() => new PlayerAdapter(adapter))
            .RegisterListener<IHeroEvents>      (() => new HeroAdapter(adapter))
            .RegisterListener<IProjectileEvents>(() => new ProjectileAdapter(adapter))
            ;
        }
    }
}