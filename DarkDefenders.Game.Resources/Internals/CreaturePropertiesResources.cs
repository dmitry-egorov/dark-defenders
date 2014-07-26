using System.Collections.Generic;
using DarkDefenders.Game.Model.EntityProperties;
using Infrastructure.DDDES;

namespace DarkDefenders.Game.Resources.Internals
{
    internal class CreaturePropertiesResources : IResources<CreatureProperties>
    {
        private readonly Dictionary<string, CreatureProperties> _map = new Dictionary<string, CreatureProperties>
        {
            { "Player", new CreatureProperties(120, 60) },
            { "Hero",   new CreatureProperties(60, 30) },
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