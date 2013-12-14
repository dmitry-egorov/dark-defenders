using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public interface IRepository<out TRoot, in TRootId>
    {
        TRoot GetById(TRootId id);
        IEnumerable<TRoot> GetAll();
    }
}