using System;
using System.Collections.Generic;

namespace Infrastructure.Util
{
    /// <summary>
    /// Extension methods for <see cref="IEnumerable{T}"/>
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Execute <paramref name="action"/> for each element of <paramref name="list"/> in reverse order.
        /// </summary>
        /// <typeparam name="T">type of elements</typeparam>
        /// <param name="list">list to iterate over</param>
        /// <param name="action">action to execute for each element</param>
        /// <exception cref="ArgumentNullException"><paramref name="list"/> or <paramref name="action"/> is null</exception>
        public static void ReverseForEach<T>(this IReadOnlyList<T> list, Action<T> action)
        {
            list.ShouldNotBeNull("list");
            action.ShouldNotBeNull("action");

            var count = list.Count;

            for (var i = count - 1; i >= 0; i--)
            {
                action(list[i]);
            }
        }

        /// <summary>
        /// Concatinate single element to <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of elements</typeparam>
        /// <param name="enumerable"><see cref="IEnumerable{T}"/> to concatinate to.</param>
        /// <param name="element">Element to concatinate</param>
        /// <returns><see cref="IEnumerable{T}"/> containing elements of original <see cref="IEnumerable{T}"/> and the <paramref name="element"/> at the end.</returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, T element)
        {
            foreach (var item in enumerable)
            {
                yield return item;
            }

            yield return element;
        }
    }
}