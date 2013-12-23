using System.Collections.Generic;
using Infrastructure.DDDES.Implementations.Domain.Exceptions;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public class Repository<TRootId, TRoot> : IRepository<TRootId, TRoot> 
        where TRootId: Identity
        where TRoot: IEntity<TRootId>
    {
        private readonly Dictionary<TRootId, TRoot> _roots = new Dictionary<TRootId, TRoot>();

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
            return _roots.Values;
        }

        public bool Exists(TRootId id)
        {
            return _roots.ContainsKey(id);
        }

        public bool TryGetById(TRootId id, out TRoot root)
        {
            return _roots.TryGetValue(id, out root);
        }

        public void Store(TRoot root)
        {
            _roots.Add(root.Id, root);
        }

        public void Remove(TRootId rootId)
        {
            _roots.Remove(rootId);
        }
    }
}