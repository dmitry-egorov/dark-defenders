namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class Entity<TEntity> : IEntity<TEntity>
    {
        private readonly IdentityOf<TEntity> _id;

        protected Entity()
        {
            _id = new IdentityOf<TEntity>();
        }

        public IdentityOf<TEntity> Id
        {
            get { return _id; }
        }
    }
}