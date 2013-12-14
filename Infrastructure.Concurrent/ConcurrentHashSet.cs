using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Infrastructure.Concurrent
{
    public class ConcurrentHashSet<TValue>
    {
        private readonly ConcurrentDictionary<TValue, byte> _dictionary = new ConcurrentDictionary<TValue, byte>();

        public bool TryAdd(TValue value)
        {
            return _dictionary.TryAdd(value, 0);
        }

        public bool TryRemove(TValue observer)
        {
            byte b;
            return _dictionary.TryRemove(observer, out b);
        }

        public IEnumerable<TValue> GetAll()
        {
            return _dictionary.Keys;
        }
    }
}
