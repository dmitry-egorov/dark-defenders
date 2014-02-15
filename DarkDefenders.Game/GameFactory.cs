using DarkDefenders.Domain.Model;
using DarkDefenders.Domain.Model.Entities.Worlds;
using DarkDefenders.Domain.Model.Other;
using DarkDefenders.Game.Interfaces;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Microsoft.Practices.Unity;

namespace DarkDefenders.Game
{
    public static class GameFactory
    {
        public static IGame Create(IEventsListener<IEventsReciever> eventsListener, IResources<Map<Tile>> mapResources, IResources<WorldProperties> propertiesResources)
        {
            var container = new UnityContainer();

            container
            .RegisterInstance(eventsListener)
            .RegisterInstance(propertiesResources)
            .RegisterInstance(mapResources)
            .RegisterGame();

            return container.ResolveGame();
        }
    }
}