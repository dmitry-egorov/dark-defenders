using System.Collections.Generic;
using Infrastructure.DDDES.Implementations.Domain.Exceptions;
using Infrastructure.Util.Collections;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public class Repository<TRootId, TRoot> : IRepository<TRootId, TRoot> 
        where TRootId: Identity
        where TRoot: IEntity<TRootId>
    {
        private readonly Dictionary<TRootId, TRoot> _rootsMap = new Dictionary<TRootId, TRoot>();
        private readonly FastList<TRoot> _allRoots = new FastList<TRoot>();

        public TRoot GetById(TRootId id)
        {
            TRoot root;
            if (!TryGetById(id, out root))
            {
                throw new RootDoesntExistException(typeof(TRoot).Name, id);
            }

            return root;
        }

        public IEnumerable<TRoot> GetAll()
        {
            return _allRoots;
        }

        public bool Exists(TRootId id)
        {
            return _rootsMap.ContainsKey(id);
        }

        public bool TryGetById(TRootId id, out TRoot root)
        {
            return _rootsMap.TryGetValue(id, out root);
        }

        public void Store(TRoot root)
        {
            _rootsMap.Add(root.Id, root);
            _allRoots.Add(root);
        }

        public void Remove(TRootId rootId)
        {
            //TODO: optimize (store index in a map)
            var root = _rootsMap[rootId];
            _rootsMap.Remove(rootId);
            _allRoots.Remove(root);
        }
    }
}