using System;
using System.Collections.Generic;

namespace Infrastructure.Util
{
    public static class RandomExtensions
    {
        public static T ElementFrom<T>(this Random random, IReadOnlyList<T> list)
        {
            var count = list.Count;
            if (count == 0)
            {
                throw new InvalidOperationException("List is empty");
            }

            if (count == 1)
            {
                return list[0];
            }

            var index = random.Next(count);

            return list[index];
        }

    }
}