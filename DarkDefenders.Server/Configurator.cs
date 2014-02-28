using DarkDefenders.Game.App;
using DarkDefenders.Game.Resources;
using DarkDefenders.Remote.AdapterFromGame;
using DarkDefenders.Server.Internals;
using Microsoft.Practices.Unity;

namespace DarkDefenders.Server
{
    public static class Configurator
    {
        public static IUnityContainer RegisterGameServer(this IUnityContainer container)
        {
            return
            container
            .ToGameBootstrapper()
            .RegisterResources()
            .RegisterRemoteEvents()
            .RegisterGameService()
            .RegisterSingleton<GameServer, IGameServer>()
            .RegisterSingleton<GameServerState>();
        }
    }
}