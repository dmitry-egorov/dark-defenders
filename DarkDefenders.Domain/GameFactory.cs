using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES;
using Microsoft.Practices.Unity;

namespace DarkDefenders.Domain
{
    public static class GameFactory
    {
        public static IGame Create(IEventsListener<EventDataBase> eventsListener)
        {
            var container = new UnityContainer();
            container.RegisterInstance(eventsListener);
            container.RegisterDomain();

            return container.ResolveGame();
        }
    }
}