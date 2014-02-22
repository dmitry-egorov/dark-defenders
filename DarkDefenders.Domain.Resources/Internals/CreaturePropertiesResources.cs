using System.Collections.Generic;
using DarkDefenders.Domain.Model.EntityProperties;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Resources.Internals
{
    internal class CreaturePropertiesResources : IResources<CreatureProperties>
    {
        private readonly Dictionary<string, CreatureProperties> _map = new Dictionary<string, CreatureProperties>
        {
            { "Player", new CreatureProperties(180, 60) },
            { "Hero",   new CreatureProperties(180, 30) },
        };

        public CreatureProperties this[string resourceId]
        {
            get
            {
                return _map[resourceId];
            }
        }
    }
}