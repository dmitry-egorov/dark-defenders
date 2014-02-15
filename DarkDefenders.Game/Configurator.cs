using DarkDefenders.Domain.Model;
using DarkDefenders.Game.Adapters;
using DarkDefenders.Game.Interfaces;
using Microsoft.Practices.Unity;

namespace DarkDefenders.Game
{
    public static class Configurator
    {
        public static IUnityContainer RegisterGame(this IUnityContainer container)
        {
            return container.RegisterDomain();
        }

        public static IGame ResolveGame(this IUnityContainer container)
        {
            return container.Resolve<GameAdapter>();
        }
    }
}