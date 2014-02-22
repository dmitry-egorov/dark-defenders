using System.Collections.Generic;
using DarkDefenders.Game.Model.EntityProperties;
using Infrastructure.DDDES;

namespace DarkDefenders.Game.Resources.Internals
{
    internal class RigidBodyPropertiesResources: IResources<RigidBodyProperties>
    {
        private readonly Dictionary<string, RigidBodyProperties> _map = new Dictionary<string, RigidBodyProperties>
        {
            { "Player",     new RigidBodyProperties(0.4f, 1.0f, 40.0f) },
            { "Hero",       new RigidBodyProperties(0.4f, 1.0f, 20.0f) },
            { "Projectile", new RigidBodyProperties(0.1f, 0.001f, 100.0f) },
        };

        public RigidBodyProperties this[string resourceId]
        {
            get
            {
                return _map[resourceId];
            }
        }
    }
}