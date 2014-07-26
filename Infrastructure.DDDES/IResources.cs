namespace Infrastructure.DDDES
{
    public interface IResources<out TResource> : IResources<string, TResource>
    {
        
    }

    public interface IResources<in TKey, out TResource>
    {
        TResource this[TKey resourceId] { get; }
    }
}