using System;
using System.Collections;
using System.Collections.Generic;

namespace Infrastructure.Util.Collections
{
    public class FastList<T>: IEnumerable<T>
    {
        private T[] _items = new T[0];
        private int _size;

        public int Count { get { return _size; } }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < _size; i++)
            {
                yield return _items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            if (_size == _items.Length)
            {
                DoubleCapacity();
            }

            _items[_size++] = item;
        }

        public bool Remove(T item)
        {
            var index = Array.IndexOf(_items, item, 0, _size);
            if (index < 0)
            {
                return false;
            }

            --_size;

            if (index < _size)
            {
                Array.Copy(_items, index + 1, _items, index, _size - index);
            }

            _items[_size] = default(T);

            return false;
        }

        private void DoubleCapacity()
        {
            var oldCapacity = _items.Length;
            var newCapacity = oldCapacity == 0 ? 4 : oldCapacity * 2;
            if ((uint)newCapacity > 2146435071U)
            {
                newCapacity = 2146435071;
            }

            var newArray = new T[newCapacity];

            if (oldCapacity > 0)
            {
                Array.Copy(_items, 0, newArray, 0, oldCapacity);
            }

            _items = newArray;
        }

        public T this[int index]
        {
            get
            {
                return _items[index];
            }
        }
    }
}