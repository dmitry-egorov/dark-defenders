using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class Destroyed<TRootId, TRoot, TDestoryedEvent> : EventBase<TRootId, TDestoryedEvent> 
        where TRootId : Identity 
        where TDestoryedEvent : EventBase<TRootId, TDestoryedEvent>
    {
        protected Destroyed(TRootId rootId) : base(rootId)
        {
        }

        protected override string ToStringInternal()
        {
            return "Root {0} destroyed: {1}".FormatWith(typeof(TRoot).Name, RootId);
        }
    }
}