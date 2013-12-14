using Infrastructure.DDDEventSourcing.Domain;

namespace Infrastructure.DDDEventSourcing.Implementations.Domain
{
    public abstract class RootBase<TState> : IRoot<TState> 
        where TState: new()
    {
        public TState State { get; private set; }

        protected RootBase()
        {
            State = new TState();
        }
    }
}