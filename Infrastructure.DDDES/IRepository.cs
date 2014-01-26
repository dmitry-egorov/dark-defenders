using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public interface IRepository<out TRoot>
    {
        IEnumerable<TRoot> GetAll();
    }
}