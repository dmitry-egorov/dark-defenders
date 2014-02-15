using System.Collections;
using System.Collections.Generic;
using Infrastructure.Util.Collections;

namespace Infrastructure.DDDES.Implementations.Internals
{
    internal class Repository<TEntity> : IReadOnlyList<TEntity>, IStorage<TEntity>
    {
        private readonly FastList<TEntity> _allEntities = new FastList<TEntity>();

        public int Count { get { return _allEntities.Count; } }

        public void Store(TEntity entity)
        {
            _allEntities.Add(entity);
        }

        public void Remove(TEntity entity)
        {
            //TODO: optimize (store index in a map)
            _allEntities.Remove(entity);
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return _allEntities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TEntity this[int index]
        {
            get { return _allEntities[index]; }
        }
    }
}