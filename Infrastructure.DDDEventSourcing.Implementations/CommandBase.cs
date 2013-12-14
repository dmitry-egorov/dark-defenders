using Infrastructure.DDDEventSourcing.Domain;

namespace Infrastructure.DDDEventSourcing.Implementations
{
    public abstract class CommandBase<TAggregateRootId> : ICommand<TAggregateRootId>
        where TAggregateRootId: Identity
    {
        private readonly TAggregateRootId _aggregateRootId;

        protected CommandBase(TAggregateRootId aggregateRootId)
        {
            _aggregateRootId = aggregateRootId;
        }

        public TAggregateRootId AggregateRootId { get { return _aggregateRootId; }}

        Identity ICommand.AggregateRootId
        {
            get { return _aggregateRootId; }
        }
    }
}