using System;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public class ActionAcceptorOf<TVisitor> : IAcceptorOf<TVisitor>
    {
        private readonly Action<TVisitor> _action;

        public ActionAcceptorOf(Action<TVisitor> action)
        {
            _action = action;
        }

        public void Accept(TVisitor visitor)
        {
            _action(visitor);
        }
    }
}