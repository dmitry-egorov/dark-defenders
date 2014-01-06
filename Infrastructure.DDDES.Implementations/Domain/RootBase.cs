namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class RootBase<TId, TRootEventsReciever, TRootEvent> : IRoot<TId, TRootEvent>
        where TRootEvent: IRootEvent<TRootEventsReciever> 
        where TId: Identity
    {
        public TId Id { get; private set; }

        public void Apply(TRootEvent rootEvent)
        {
            var eventReciever = (TRootEventsReciever)(object)this;

            rootEvent.ApplyTo(eventReciever);
        }

        protected RootBase(TId id)
        {
            Id = id;
        }
    }
}