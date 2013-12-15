using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

namespace Infrastructure.Util
{
    public static class DictionaryExtensions
    {
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