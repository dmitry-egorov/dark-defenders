using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Infrastructure.Runtime.Collections
{
    public class ConcurrentHashSet<T>: IEnumerable<T>
    {
        private readonly ConcurrentDictionary<T, byte> _dict = new ConcurrentDictionary<T, byte>();

        public bool TryAdd(T item)
        {
            return _dict.TryAdd(item, 1);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _dict.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(T item)
        {
            return _dict.ContainsKey(item);
        }

        public bool TryRemove(T item)
        {
            byte value;
            return _dict.TryRemove(item, out value);
        }
    }
}