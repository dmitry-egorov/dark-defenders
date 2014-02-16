using DarkDefenders.Domain.Game;

namespace DarkDefenders.Domain.Resources
{
    public static class Configurator
    {
        public static GameBootstrapper RegisterResources(this GameBootstrapper bootstrapper)
        {
            return bootstrapper
                .RegisterResource(new MapResources())
                .RegisterResource(new WorldPropertiesResources())
                .RegisterResource(new RigidBodyPropertiesResources())
                .RegisterResource(new CreaturePropertiesResources());
        }
    }
}