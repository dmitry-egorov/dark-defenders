using Microsoft.Practices.Unity;

namespace DarkDefenders.Game.App
{
    public static class GameBootstrapperExtensions
    {
        public static GameBootstrapper ToGameBootstrapper(this IUnityContainer container)
        {
            return new GameBootstrapper(container);
        }
    }
}