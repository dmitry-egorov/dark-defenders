using System.Collections.ObjectModel;
using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public class CompositeStorage<T>: IStorage<T>
    {
        private readonly ReadOnlyCollection<IStorage<T>> _storages;

        public CompositeStorage(params IStorage<T>[] storages)
        {
            _storages = storages.AsReadOnly();
        }


        public void Store(T item)
        {
            foreach (var storage in _storages)
            {
                storage.Store(item);
            }
        }

        public void Remove(T item)
        {
            foreach (var storage in _storages)
            {
                storage.Remove(item);
            }
        }
    }
}