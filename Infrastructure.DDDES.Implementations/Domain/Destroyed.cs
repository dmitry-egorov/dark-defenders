using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class Destroyed<TEntity, TReciever> : EventOf<TEntity, TReciever> 
        where TEntity : IEntity<TEntity>
    {
        private readonly IdentityOf<TEntity> _entityId;
        private readonly IStorage<TEntity> _storage;

        protected Destroyed(TEntity entity, IStorage<TEntity> storage) : base(entity)
        {
            _entityId = entity.Id;
            _storage = storage;
        }

        public override string ToString()
        {
            return "Entity {0}:{1} destroyed".FormatWith(typeof(TEntity).Name, _entityId);
        }

        protected override void Apply(TEntity entity)
        {
            _storage.Remove(entity);
        }
    }
}