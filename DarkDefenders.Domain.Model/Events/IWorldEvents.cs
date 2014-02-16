using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Events
{
    public interface IWorldEvents: IEntityEvents
    {
        void Created();
    }
}