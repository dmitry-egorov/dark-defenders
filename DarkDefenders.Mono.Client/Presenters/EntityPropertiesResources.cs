using System.Collections.Generic;
using DarkDefenders.Remote.Model;
using Infrastructure.DDDES;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DarkDefenders.Mono.Client.Presenters
{
    public class EntityPropertiesResources: IResources<RemoteEntityType, EntityProperties>
    {
        private readonly Dictionary<RemoteEntityType, EntityProperties> _map;

        public EntityPropertiesResources(ContentManager contentManager)
        {
            var white = contentManager.Load<Texture2D>("white");
            var dwarf = contentManager.Load<Texture2D>("dwarf");

            _map = new Dictionary<RemoteEntityType, EntityProperties>
            {
                { RemoteEntityType.Player    , new EntityProperties(0.8f, 0.8f, white, Color.Blue) },
                { RemoteEntityType.Hero      , new EntityProperties(5f  , 5f  , dwarf, Color.White) },
                { RemoteEntityType.Projectile, new EntityProperties(0.8f, 0.8f, white, Color.Purple) },
            };
        }

        public EntityProperties this[RemoteEntityType resourceId]
        {
            get
            {
                return _map[resourceId];
            }
        }
    }
}