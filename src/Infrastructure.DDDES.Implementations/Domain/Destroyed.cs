using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public sealed class Destroyed<TEntity, TEntityEvents>: IEvent
        where TEntity : IEntity<TEntity>, TEntityEvents
        where TEntityEvents: IEntityEvents
    {
        private readonly TEntity _entity;
        private readonly TEntityEvents _externalReciever;
        private readonly IStorage<TEntity> _storage;

        public Destroyed(TEntity entity, TEntityEvents externalReciever, IStorage<TEntity> storage)
        {
            _entity = entity;
            _externalReciever = externalReciever;
            _storage = storage;
        }

        public override string ToString()
        {
            return "Entity {0}:{1} destroyed".FormatWith(typeof(TEntity).Name, _entity.Id);
        }

        public void Apply()
        {
            _entity.Destroyed();
            _externalReciever.Destroyed();

            _storage.Remove(_entity);
        }
    }
}