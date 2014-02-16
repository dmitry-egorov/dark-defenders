using DarkDefenders.Domain.Game;
using DarkDefenders.Domain.Model.Events;
using DarkDefenders.Remote.Model.Interface;
using DarkDefenders.Remote.Model.Internals;
using DarkDefenders.Remote.Model.Internals.Entities;
using Infrastructure.DDDES;

namespace DarkDefenders.Remote.Model
{
    public static class Configurator
    {
        public static GameBootstrapper RegisterRemoteEvents(this GameBootstrapper bootstrapper, IEventsListener<IRemoteEvents> listener)
        {
            var packer = new RemoteEventsPacker(listener);
            var adapter = new RemoteEventAdapter(packer);

            return bootstrapper
            .RegisterListener<IClockEvents>     (() => new RemoteClock(packer))
            .RegisterListener<ITerrainEvents>   (() => new RemoteTerrain(packer))
            .RegisterListener<IRigidBodyEvents> (() => new RemoteRigidBody(adapter))
            .RegisterListener<ICreatureEvents>  (() => new RemoteCreature(adapter))
            .RegisterListener<IPlayerEvents>    (() => new RemotePlayer(adapter))
            .RegisterListener<IHeroEvents>      (() => new RemoteHero(adapter))
            .RegisterListener<IProjectileEvents>(() => new RemoteProjectile(adapter))
            ;
        }
    }
}