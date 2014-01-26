namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class Entity<TId> : IEntity<TId> 
        where TId : new()
    {
        private readonly TId _globalId;

        protected Entity()
        {
            _globalId = new TId();
        }

        public TId GetGlobalId()
        {
            return _globalId;
        }
    }
}