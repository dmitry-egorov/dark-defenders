using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public interface IRepository<in TRootId, out TRoot>
    {
        TRoot GetById(TRootId id);
        IEnumerable<TRoot> GetAll();
        bool Exists(TRootId id);
    }
}