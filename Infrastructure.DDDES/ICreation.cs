using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public interface ICreation<out TEntity> : IEnumerable<IEvent>, IContainer<TEntity>
    {
        
    }
}