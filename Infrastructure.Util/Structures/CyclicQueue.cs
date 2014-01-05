using System.Collections;
using System.Collections.Generic;

namespace Infrastructure.Util.Structures
{
    public class CyclicQueue<T>: IEnumerable<T>
    {
        private List<T> _items = new List<T>();
        public int Count { get { return _items.Count; }}

        public void Enqueue(T item)
        {
            _items.Add(item);
        }

        public void Enqueue(IEnumerable<T> items)
        {
            _items.AddRange(items);
        }

        public void Clear()
        {
            _items = new List<T>(_items.Capacity);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}