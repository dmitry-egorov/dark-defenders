using System;
using System.Collections.Generic;

namespace Infrastructure.Util
{
    public static class DictionaryExtensions
    {
        public static void AddIfNotExists<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TValue> creator)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, creator());
            }
        }

        public static TValue GetOrCreate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TValue> creator)
        {
            TValue value;

            if (!dictionary.TryGetValue(key, out value))
            {
                value = creator();
                dictionary.Add(key, value);
            }

            return value;
        }
    }
}