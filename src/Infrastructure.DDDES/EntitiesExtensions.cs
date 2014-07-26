using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public static class EntitiesExtensions
    {
        public static void ForAll<TEntity>(this IEnumerable<TEntity> entities, Action<TEntity> command)
        {
            foreach (var entity in entities)
            {
                command(entity);
            }
        }
    }
}