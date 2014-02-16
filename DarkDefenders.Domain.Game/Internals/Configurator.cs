using DarkDefenders.Domain.Game.Adapters;
using DarkDefenders.Domain.Game.Interfaces;
using Microsoft.Practices.Unity;

namespace DarkDefenders.Domain.Game.Internals
{
    internal static class Configurator
    {
        public static IGame ResolveGame(this IUnityContainer container)
        {
            return container.Resolve<GameAdapter>();
        }
    }
}