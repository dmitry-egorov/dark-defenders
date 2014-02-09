using System.Collections.Generic;
using System.Linq;
using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public static class CreationExtensions
    {
        public static ICreation<T> Concat<T>(this ICreation<T> creation, IEnumerable<IEvent> enumerable) 
            where T : class
        {
            var events = creation.AsEnumerable().Concat(enumerable);

            return new Creation<T>(events, creation);
        }

        public static ICreation<T> ConcatEvent<T>(this ICreation<T> creation, IEvent e) 
            where T : class
        {
            var events = creation.AsEnumerable().ConcatItem(e);

            return new Creation<T>(events, creation);
        }
    }
}