using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.DDDES
{
    public static class RepositoryExtensions
    {
        public static IEnumerable<IEvent> ForAll<TRoot>(this IRepository<TRoot> repository, Func<TRoot, IEnumerable<IEvent>> command)
        {
            var entities = repository.GetAll();
            return entities.Select(command).SelectMany(e => e);
        }
    }
}