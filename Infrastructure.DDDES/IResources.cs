namespace Infrastructure.DDDES
{
    public interface IResources<out TResource>
    {
        TResource this[string resourceId] { get; }
    }
}