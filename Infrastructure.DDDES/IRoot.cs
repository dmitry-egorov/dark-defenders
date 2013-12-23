namespace Infrastructure.DDDES
{
    public interface IRoot<out TId, in TRootEvent> : IEntity<TId>
    {
        void Apply(TRootEvent rootEvent);
    }
}