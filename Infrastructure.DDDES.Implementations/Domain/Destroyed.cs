using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations.Domain
{

    public abstract class Destroyed<TRoot, TId, TEventDto> : Event<TRoot, TId, TEventDto> 
        where TRoot : IEntity<TId>
    {
        private readonly TId _rootId;
        private readonly IStorage<TRoot> _storage;

        protected Destroyed(TRoot root, IStorage<TRoot> storage) : base(root)
        {
            _rootId = root.GetGlobalId();
            _storage = storage;
        }

        public override string ToString()
        {
            return "Root {0} destroyed: {1}".FormatWith(typeof(TRoot).Name, _rootId);
        }

        protected override void Apply(TRoot root)
        {
            _storage.Remove(root);
        }
    }
}