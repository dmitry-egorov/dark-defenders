using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Infrastructure.Util
{
    /// <summary>
    /// Extension methods for <see cref="ReadOnlyCollection{T}"/>
    /// </summary>
    public static class ReadOnlyCollectionExtensions
    {
        /// <summary>
        /// Cast or copy <paramref name="enumerable"/> to a new <see cref="ReadOnlyCollection{T}"/>
        /// </summary>
        /// <typeparam name="T">Elements' type</typeparam>
        /// <param name="enumerable">Enumerable to convert to <see cref="ReadOnlyCollection{T}"/></param>
        /// <returns>New <see cref="ReadOnlyCollection{T}"/> filled with items from <paramref name="enumerable"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is null</exception>
        public static ReadOnlyCollection<T> AsReadOnly<T>(this IEnumerable<T> enumerable)
        {
            enumerable.ShouldNotBeNull("enumerable");

            var readOnlyCollection = enumerable as ReadOnlyCollection<T>;
            if (readOnlyCollection != null)
            {
                return readOnlyCollection;
            }

            var list = enumerable as IList<T>;
            if (list != null)
            {
                return new ReadOnlyCollection<T>(list);
            }

            return new ReadOnlyCollection<T>(enumerable.ToList());
        }
        
        /// <summary>
        /// Creates a <see cref="ReadOnlyCollection{T}"/> from provided <paramref name="list"/>
        /// </summary>
        /// <typeparam name="T">Elements' type</typeparam>
        /// <param name="list">List to create <see cref="ReadOnlyCollection{T}"/> from</param>
        /// <returns>New <see cref="ReadOnlyCollection{T}"/> with underlying <paramref name="list"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="list"/> is null</exception>
        public static ReadOnlyCollection<T> AsReadOnly<T>(this IList<T> list)
        {
            list.ShouldNotBeNull("list");

            var readOnlyCollection = list as ReadOnlyCollection<T>;
            if (readOnlyCollection != null)
            {
                return readOnlyCollection;
            }

            return new ReadOnlyCollection<T>(list);
        }
    }
}
