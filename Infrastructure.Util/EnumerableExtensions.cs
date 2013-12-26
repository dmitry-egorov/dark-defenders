using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

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

        public static IEnumerable<T> EnumerateOnce<T>(this T item)
        {
            yield return item;
        }

        public static IEnumerable<T> HeadAndTail<T>(this IEnumerable<T> enumerable, out T head)
        {
            using (var enumerator = enumerable.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    throw new InvalidOperationException("Enumerable doesn't contain any elements.");
                }

                head = enumerator.Current;

                return Yield(enumerator);
            }
        }

        private static IEnumerable<T> Yield<T>(IEnumerator<T> enumerator)
        {
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }

        public static Iterator<TElement> GetIterator<TElement>(this IEnumerable<TElement> enumerable)
        {
            return new Iterator<TElement>(enumerable.GetEnumerator());
        }

        /// <summary>
        /// Fast adjacent grouping. Requiers enumeration of each grouping before next grouping is requested.
        /// </summary>
        public static IEnumerable<IGrouping<TKey, TElement>> GroupAdjacentFast<TKey, TElement>(this IEnumerable<TElement> enumerable, Func<TElement, TKey> keySelector)
        {
            using (var enumerator = enumerable.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    yield break;
                }

                var value = enumerator.Current;
                var key = keySelector(value);

                while (true)
                {
                    var grouping = new FastGrouping<TKey, TElement>(value, key, enumerator, keySelector);

                    yield return grouping;

                    if (grouping.IsEnumeratorFinished())
                    {
                        yield break;
                    }

                    grouping.AssertGroupFinished();

                    key = grouping.GetNextKey();
                    value = grouping.GetNextValue();
                }
            }
        }

        private class FastGrouping<TKey, TElement> : IGrouping<TKey, TElement>
        {
            private readonly TElement _firstValue;
            private readonly IEnumerator<TElement> _enumerator;
            private readonly Func<TElement, TKey> _keySelector;
            private bool _groupFinished;
            private bool _enumeratorFinished;
            private TKey _nextKey;
            private TElement _nextValue;

            public FastGrouping(TElement firstValue, TKey key, IEnumerator<TElement> enumerator, Func<TElement, TKey> keySelector)
            {
                Key = key;
                _firstValue = firstValue;
                _enumerator = enumerator;
                _keySelector = keySelector;
            }

            public IEnumerator<TElement> GetEnumerator()
            {
                yield return _firstValue;

                while (true)
                {
                    if (!_enumerator.MoveNext())
                    {
                        _enumeratorFinished = true;
                        yield break;
                    }

                    var nextValue = _enumerator.Current;
                    var newKey = _keySelector(nextValue);
                    if (!newKey.Equals(Key))
                    {
                        _nextKey = newKey;
                        _nextValue = nextValue;
                        _groupFinished = true;
                        yield break;
                    }

                    yield return nextValue;
                }

            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public TKey Key { get; private set; }

            [Conditional("DEBUG")]
            public void AssertGroupFinished()
            {
                if (!_groupFinished)
                {
                    throw new InvalidOperationException("Immediate enumeration of groups is required");
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool IsEnumeratorFinished()
            {
                return _enumeratorFinished;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TKey GetNextKey()
            {
                return _nextKey;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TElement GetNextValue()
            {
                return _nextValue;
            }
        }
    }

    
}