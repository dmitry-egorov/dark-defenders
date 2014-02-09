using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class Destroyed<TEntity, TReciever> : EventOf<TEntity, TReciever> 
        where TEntity : IEntity<TEntity>
    {
        private readonly IdentityOf<TEntity> _rootId;
        private readonly IStorage<TEntity> _storage;

        protected Destroyed(TEntity root, IStorage<TEntity> storage) : base(root)
        {
            _rootId = root.Id;
            _storage = storage;
        }

        public override string ToString()
        {
            return "Root {0} destroyed: {1}".FormatWith(typeof(TEntity).Name, _rootId);
        }

        protected override void Apply(TEntity root)
        {
            _storage.Remove(root);
        }
    }
}