using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public class Removed<TRootId, TRemovedEvent> : EventBase<TRootId, TRemovedEvent> 
        where TRootId : Identity 
        where TRemovedEvent : EventBase<TRootId, TRemovedEvent>
    {
        public Removed(TRootId rootId) : base(rootId)
        {
        }

        protected override string ToStringInternal()
        {
            return "Root removed: {0}".FormatWith(RootId);
        }

        protected override bool EventEquals(TRemovedEvent other)
        {
            return true;
        }

        protected override int GetEventHashCode()
        {
            return 1;
        }
    }
}