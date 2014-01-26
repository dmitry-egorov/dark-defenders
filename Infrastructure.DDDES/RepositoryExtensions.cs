using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.DDDES
{
    public static class RepositoryExtensions
    {
        public static IEnumerable<TEvent> ForAll<TRoot, TEvent>(this IRepository<TRoot> repository, Func<TRoot, IEnumerable<TEvent>> command)
        {
            var entities = repository.GetAll();
            return entities.Select(command).SelectMany(e => e);
        }
    }
}