using System.Collections;
using System.Collections.Generic;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public class Creation<T> : ICreation<T> 
        where T : class
    {
        private readonly IEnumerable<IEvent> _events;
        private readonly IContainer<T> _container;

        public Creation(IEnumerable<IEvent> events, IContainer<T> container)
        {
            _events = events;
            _container = container;
        }

        public IEnumerator<IEvent> GetEnumerator()
        {
            return _events.GetEnumerator();
        }

        public T Entity
        {
            get { return _container.Entity; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}