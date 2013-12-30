namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class RootBase<TId, TRootEventReciever, TRootEvent> : IRoot<TId, TRootEvent>
        where TRootEvent: IRootEvent<TRootEventReciever> 
        where TId: Identity
    {
        public TId Id { get; private set; }

        public void Apply(TRootEvent rootEvent)
        {
            var eventReciever = (TRootEventReciever)(object)this;

            rootEvent.ApplyTo(eventReciever);
        }

        protected RootBase(TId id)
        {
            Id = id;
        }
    }
}