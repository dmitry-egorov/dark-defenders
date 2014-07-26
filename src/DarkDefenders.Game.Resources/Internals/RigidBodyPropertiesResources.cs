using System.Collections.Generic;
using DarkDefenders.Game.Model.EntityProperties;
using Infrastructure.DDDES;

namespace DarkDefenders.Game.Resources.Internals
{
    internal class RigidBodyPropertiesResources: IResources<RigidBodyProperties>
    {
        private readonly Dictionary<string, RigidBodyProperties> _map = new Dictionary<string, RigidBodyProperties>
        {
            { "Player",     new RigidBodyProperties(0.4, 1    , 35 ) },
            { "Hero",       new RigidBodyProperties(0.4, 1.0f , 10 ) },
            { "Projectile", new RigidBodyProperties(0.1, 0.001, 100) },
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