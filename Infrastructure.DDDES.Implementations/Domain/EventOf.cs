namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class EventOf<TEntity, TReciever> : IEvent, IAcceptorOf<TReciever> 
        where TEntity: IEntity<TEntity>
    {
        private readonly TEntity _root;
        protected EventOf(TEntity root)
        {
            _root = root;
        }

        public void Apply()
        {
            Apply(_root);
        }

        public void Accept(TReciever reciever)
        {
            Accept(reciever, _root.Id);
        }

        protected abstract void Accept(TReciever reciever, IdentityOf<TEntity> id);
        protected abstract void Apply(TEntity root);
    }
}