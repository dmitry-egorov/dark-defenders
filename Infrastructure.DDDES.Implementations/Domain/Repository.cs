using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Util.Collections;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public class Repository<TEntity> : IReadOnlyCollection<TEntity>, IStorage<TEntity>
    {
        private readonly FastList<TEntity> _allEntities = new FastList<TEntity>();

        public int Count { get { return _allEntities.Count; } }

        public void Store(TEntity item)
        {
            _allEntities.Add(item);
        }

        public void Remove(TEntity item)
        {
            //TODO: optimize (store index in a map)
            _allEntities.Remove(item);
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return _allEntities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}