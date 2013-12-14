using Infrastructure.DDDEventSourcing.Domain;

namespace Infrastructure.DDDEventSourcing.Implementations.Domain
{
    public abstract class CommandBase<TRootId> : ICommand<TRootId>
        where TRootId: Identity
    {
        private readonly TRootId _rootId;

        protected CommandBase(TRootId rootId)
        {
            _rootId = rootId;
        }

        public TRootId RootId { get { return _rootId; }}

        Identity ICommand.RootId
        {
            get { return _rootId; }
        }
    }
}