﻿using DarkDefenders.Game.App;
using DarkDefenders.Game.Resources.Internals;

namespace DarkDefenders.Game.Resources
{
    public static class Configurator
    {
        public static GameBootstrapper RegisterResources(this GameBootstrapper bootstrapper)
        {
            return bootstrapper
                .RegisterFileResources()
                .RegisterHardcodedResources()
                ;
        }

        public static GameBootstrapper RegisterFileResources(this GameBootstrapper bootstrapper)
        {
            return bootstrapper
            .RegisterResource(new MapResources())
            .RegisterResource(new WorldPropertiesResources())
            ;
        }

        public static GameBootstrapper RegisterHardcodedResources(this GameBootstrapper bootstrapper)
        {
            return bootstrapper
            .RegisterResource(new RigidBodyPropertiesResources())
            .RegisterResource(new CreaturePropertiesResources())
            ;
        }
    }
}