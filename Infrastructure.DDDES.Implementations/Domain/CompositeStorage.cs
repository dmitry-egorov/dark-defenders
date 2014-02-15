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


        public void Store(T entity)
        {
            foreach (var storage in _storages)
            {
                storage.Store(entity);
            }
        }

        public void Remove(T entity)
        {
            foreach (var storage in _storages)
            {
                storage.Remove(entity);
            }
        }
    }
}