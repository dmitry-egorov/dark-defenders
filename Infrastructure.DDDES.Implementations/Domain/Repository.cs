using System.Collections.Generic;
using Infrastructure.Util.Collections;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public class Repository<TRoot> : IRepository<TRoot>, IStorage<TRoot>
    {
        private readonly FastList<TRoot> _allRoots = new FastList<TRoot>();

        public IEnumerable<TRoot> GetAll()
        {
            return _allRoots;
        }

        public void Store(TRoot item)
        {
            _allRoots.Add(item);
        }

        public void Remove(TRoot item)
        {
            //TODO: optimize (store index in a map)
            _allRoots.Remove(item);
        }
    }
}