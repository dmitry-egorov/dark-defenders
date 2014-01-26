
namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class Event<TRoot, TId, TEventDto> : IEvent
        where TRoot: IEntity<TId>
    {
        private readonly TRoot _root;

        protected Event(TRoot root)
        {
            _root = root;
        }

        public void Apply()
        {
            Apply(_root);
        }

        protected abstract void Apply(TRoot root);

        public virtual object ToDto()
        {
            var id = _root.GetGlobalId();
            return CreateDto(id);
        }

        protected abstract TEventDto CreateDto(TId id);
    }
}