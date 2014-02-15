namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class EventOf<TEntity, TReciever> : IEvent, IAcceptorOf<TReciever>
        where TEntity: IEntity<TEntity>
    {
        private readonly TEntity _entity;
        protected EventOf(TEntity entity)
        {
            _entity = entity;
        }

        public void Apply()
        {
            Apply(_entity);
        }

        public void Accept(TReciever reciever)
        {
            Accept(reciever, _entity.Id);
        }

        protected abstract void Accept(TReciever reciever, IdentityOf<TEntity> id);
        protected abstract void Apply(TEntity entity);
    }
}