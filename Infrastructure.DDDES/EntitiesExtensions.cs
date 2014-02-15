using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.DDDES
{
    public static class EntitiesExtensions
    {
        public static IEnumerable<IEvent> ForAll<TEntity>(this IEnumerable<TEntity> entities, Func<TEntity, IEnumerable<IEvent>> command)
        {
            return entities.Select(command).SelectMany(e => e);
        }
    }
}