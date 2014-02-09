using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public interface ICreation<out T> : IEnumerable<IEvent>, IContainer<T>
    {
        
    }
}