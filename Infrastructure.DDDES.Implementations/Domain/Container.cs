using System;
using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public class Container<T>: IContainer<T>, IStorage<T> 
        where T : class
    {
        private T _item;

        public T Entity
        {
            get
            {
                if (ReferenceEquals(_item, null))
                {
                    throw new InvalidOperationException("Item is not set");
                }
                return _item;
            }
        }

        public void Store(T item)
        {
            if (!ReferenceEquals(_item, null))
            {
                throw new InvalidOperationException("Item is already stored.");
            }

            _item = item.ShouldNotBeNull("item");
        }

        public void Remove(T item)
        {
            if (!ReferenceEquals(Entity, item))
            {
                throw new InvalidOperationException("Item is not in the container");
            }

            _item = null;
        }
    }
}