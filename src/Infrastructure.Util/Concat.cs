using System.Collections.Generic;

namespace Infrastructure.Util
{
    public static class Concat
    {
        public static IEnumerable<T> All<T>(params IEnumerable<T>[] enumerables)
        {
            foreach (var enumerable in enumerables)
            {
                foreach (var item in enumerable)
                {
                    yield return item;
                }
            }
        }
    }
}