
using Infrastructure.Data;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class Event<TEntity> : IEvent
        where TEntity: IEntity<TEntity>
    {
        private readonly TEntity _root;

        protected Event(TEntity root)
        {
            _root = root;
        }

        public void Apply()
        {
            Apply(_root);
        }

        public object GetData()
        {
            var id = _root.Id;
            return CreateData(id);
        }

        protected abstract object CreateData(IdentityOf<TEntity> id);

        protected abstract void Apply(TEntity root);
    }
}