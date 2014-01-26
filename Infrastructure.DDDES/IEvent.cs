namespace Infrastructure.DDDES
{
    public interface IEvent
    {
        void Apply();
        object ToDto();
    }
}