﻿using DarkDefenders.Game.App.Interfaces;
using DarkDefenders.Game.App.Services;
using Microsoft.Practices.Unity;

namespace DarkDefenders.Game.App.Internals
{
    internal static class Configurator
    {
        public static IGameService ResolveGame(this IUnityContainer container)
        {
            return container.Resolve<GameService>();
        }
    }
}