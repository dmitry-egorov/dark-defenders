using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public interface IRoot<in TRootEvent>
    {
        void Apply(IEnumerable<TRootEvent> events);
    }
}